using Godot;
using System;

public class AI : Node {
    public Camp Camp;
    public int UnitRange;
    public int LaneRange;
    int UnitSelect;
    int LaneSelect;
    bool SpawnReady = false;
    Timer InhibitorTimer;

    public override void _Ready() {
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
        if (Camp.Cursor.Lane < LaneSelect) {
            Camp.Cursor.MoveDown();
        } else if (Camp.Cursor.Lane > LaneSelect) {
            Camp.Cursor.MoveUp();
        } else if (Camp.SpawnTimerContainer.UnitSelect < UnitSelect) {
            Camp.SpawnTimerContainer.SelectSpawn(1);
        } else if (Camp.SpawnTimerContainer.UnitSelect > UnitSelect) {
            Camp.SpawnTimerContainer.SelectSpawn(-1);
        } else if (Camp.SpawnTimerContainer.CheckTimer(UnitSelect)) {
            SpawnReady = true;
        }
    }

    void NewMove() {
        GD.Randomize();
        UnitSelect = (int)GD.RandRange(0, UnitRange-0.0001);
        LaneSelect = (int)GD.RandRange(0, LaneRange-0.0001);
    }

}