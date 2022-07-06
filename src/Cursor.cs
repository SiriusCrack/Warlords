using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite
{
    Array<float> Lanes = new Array<float>();
    [Export] Array<PackedScene> UnitScenes;
    Array<Timer> SpawnTimers = new Array<Timer>();
    [Export] Unit.Side MySide;
    Vector2 Direction;
    [Export] public bool playable;
    int MyLane = 0;
    int UnitSelect = 0;
    Node SpawnPool;
    Timer AITimer;
    int AILane;

    public override void _Ready() {
        GD.Randomize();
        SpawnPool = GetParent().GetNode("Units");
        AITimer = GetNode<Timer>("AITimer");
        SetDirection();
        SetLanes();
        SetSpawnTimers();
        if(!playable) {
            AINewMove();
        }
    }

    public override void _Process(float delta) {
        if (playable) {
            GetInput();
        } else if (AITimer.IsStopped()) {
            AI();
        }
    }

    private void GetInput() {
        if (Input.IsActionJustPressed("right")) {
            SelectSpawn(true);
        }
        if (Input.IsActionJustPressed("left")) {
            SelectSpawn(false);
        }
        if (Input.IsActionJustPressed("spawn")) {
            SpawnUnit(UnitSelect);
        }
        if (Input.IsActionJustPressed("up")) {
            MoveUp();
        }
        if (Input.IsActionJustPressed("down")) {
            MoveDown();
        }
    }

    private void MoveUp() {
        if (MyLane > 0) {
            MyLane--;
            Position = new Vector2(Position.x, Lanes[MyLane]);
        }
    }

    private void MoveDown() {
        if (MyLane < Lanes.Count-1) {
            MyLane++;
            Position = new Vector2(Position.x, Lanes[MyLane]);
        }
    }
    private void SelectSpawn(bool right) {
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = false;
        if (right) {
            UnitSelect++;
            UnitSelect = UnitSelect %SpawnTimers.Count;
        } else {
            UnitSelect--;
            if (UnitSelect < 0) {
                UnitSelect = SpawnTimers.Count-1;
            }
        }
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = true;
    }

    private void SpawnUnit(int arg) {
        if (SpawnTimers[arg].IsStopped()) {
            Node unitType = UnitScenes[arg].Instance();
            Unit unit = unitType.GetNode<Unit>("Unit");
            unitType.RemoveChild(unit);
            SpawnPool.AddChild(unit);
            unitType.QueueFree();
            unit.MySide = MySide;
            if (MySide == Unit.Side.Left) {
                unit.Direction = Vector2.Right;
            } else {
                unit.Direction = Vector2.Left;
                unit.Scale = new Vector2 (-1, 1);
            }
            unit.GlobalPosition = this.GlobalPosition + (Direction * 75);
            SpawnTimers[arg].Start();
            int index = 0;
            foreach (Timer spawnTimer in SpawnTimers) {
                SpawnTimers[index].Start();
                index++;
            }
        }
    }

    void SetDirection() {
        if (MySide == Unit.Side.Left) {
            Direction = Vector2.Left;
        } else {
            Direction = Vector2.Right;
        }
    }

    void SetLanes() {
        Vector2 displaySize = GetViewport().GetVisibleRect().Size;
        float bottom = displaySize.y * -0.05f;
        float step = (displaySize.y - 100 + bottom) / -9f;
        for (float i = 8; i > -1; i--) {
            Lanes.Add(bottom + (step*i));
        }
        float cursorGap = displaySize.x*0.03f;
        if (MySide == Unit.Side.Right) {
            cursorGap = cursorGap * -1f;
        }
        this.Position = (new Vector2(cursorGap, Lanes[0]));
    }

    void SetSpawnTimers() {
        Node spawnTimerContainerUI;
        if (MySide == Unit.Side.Left) {
            spawnTimerContainerUI = Owner.GetNode("UI/VBoxContainer/MarginContainer/HBoxContainer/LSpawnTimerContainerUI");
        } else {
            spawnTimerContainerUI = Owner.GetNode("UI/VBoxContainer/MarginContainer/HBoxContainer/RSpawnTimerContainerUI");
        }
        foreach (PackedScene unitScene in UnitScenes) {
            Node unitType = unitScene.Instance();
            Node spawnTimerUI = unitType.GetNode("SpawnTimerUI");
            unitType.RemoveChild(spawnTimerUI);
            spawnTimerContainerUI.AddChild(spawnTimerUI);
            unitType.QueueFree();
            SpawnTimers.Add(spawnTimerUI.GetNode<Timer>("SpawnTimer"));
        }
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = true;
    }

    private void AINewMove() {
        GD.Randomize();
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = false;
        UnitSelect = (int)(GD.Randi()%3);
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = true;
    }

    private void AI() {
        if (SpawnTimers[UnitSelect].IsStopped()) {
            SpawnUnit(UnitSelect);
            AILane = (int)(GD.Randi()%9);
            AINewMove();
        }
        if (AILane < MyLane) {
            MoveUp();
        }
        if (AILane > MyLane) {
            MoveDown();
        }
        AITimer.Start();
    }
}
