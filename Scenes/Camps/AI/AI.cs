using Godot;
using System;

public class AI : Node {
    Camp Camp;
    Cursor Cursor;
    SpawnTimerContainer SpawnTimerContainer;
    int UnitRange;
    int LaneRange;
    int UnitSelect;
    int LaneSelect;
    bool SpawnReady;
    Timer InhibitorTimer;

    public void SetUp (
        Camp camp,
        int unitRange,
        int laneRange
    ) {
        Camp = camp;
        UnitRange = unitRange;
        LaneRange = laneRange;
        SpawnReady = false;
    }

    public override void _Ready() {
        Cursor = Camp.GetCursor();
        SpawnTimerContainer = Camp.GetSpawnTimerContainer();
        InhibitorTimer = GetNode<Timer>("InhibitorTimer");
        NewMove();
    }

    public override void _Process(float delta) {
        if (InhibitorTimer.IsStopped()) {
            Move();
            InhibitorTimer.Start();
        }
        if (SpawnReady) {
            Spawn();
        }
    }
    
    void Spawn() {
        SpawnReady = false;
        Camp.SpawnUnit(UnitSelect);
        NewMove();
    }

    void Move() {
        if (Cursor.GetLane() < LaneSelect) {
            Cursor.MoveDown();
        } else if (Cursor.GetLane() > LaneSelect) {
            Cursor.MoveUp();
        } else if (SpawnTimerContainer.GetUnitSelect() < UnitSelect) {
            SpawnTimerContainer.SpawnSelectRight();
        } else if (SpawnTimerContainer.GetUnitSelect() > UnitSelect) {
            SpawnTimerContainer.SpawnSelectLeft();
        } else if (SpawnTimerContainer.CheckTimer(UnitSelect)) {
            SpawnReady = true;
        }
    }

    void NewMove() {
        GD.Randomize();
        UnitSelect = (int)GD.RandRange(0, UnitRange-0.0001);
        LaneSelect = (int)GD.RandRange(0, LaneRange-0.0001);
    }

}