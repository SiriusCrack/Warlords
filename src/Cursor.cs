using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite
{
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

    public void SetUp() {
        GD.Randomize();
        SpawnPool = GetParent().GetNode("Units");
        AITimer = GetNode<Timer>("AITimer");
        SetDirection();
        if (OS.HasTouchscreenUiHint()) {
            SetSpawnTimersTouch();
        } else {
            SetSpawnTimers();
        }
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
            Position = new Vector2(Position.x, GetOwner<Battle>().Lanes[MyLane]);
        }
    }

    private void MoveDown() {
        if (MyLane < GetOwner<Battle>().Lanes.Count-1) {
            MyLane++;
            Position = new Vector2(Position.x, GetOwner<Battle>().Lanes[MyLane]);
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

    public void SpawnUnit(int arg) {
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

    void SetSpawnTimers() {
        Node spawnTimerContainerUI;
        if (MySide == Unit.Side.Left) {
            spawnTimerContainerUI = Owner.GetNode("UI/VBoxContainer/MarginContainer/HBoxContainer/LSpawnTimerContainerUI");
        } else {
            spawnTimerContainerUI = Owner.GetNode("UI/VBoxContainer/MarginContainer/HBoxContainer/RSpawnTimerContainerUI");
        }
        int addr = 0;
        foreach (PackedScene unitScene in UnitScenes) {
            Node unitType = unitScene.Instance();
            SpawnTimerUI spawnTimerUI = unitType.GetNode<SpawnTimerUI>("SpawnTimerUI");
            unitType.RemoveChild(spawnTimerUI);
            spawnTimerContainerUI.AddChild(spawnTimerUI);
            unitType.QueueFree();
            spawnTimerUI.MyCursor = this;
            spawnTimerUI.TimerAddress = addr;
            addr++;
            SpawnTimers.Add(spawnTimerUI.GetNode<Timer>("SpawnTimer"));
        }
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = true;
    }

    void SetSpawnTimersTouch() {
        if (playable) {
            Control spawnControlsContainer = Owner.GetNode<Control>("TouchInput/SpawnControls/HBoxContainer");
            // spawnControlsContainer.RectScale *= new Vector2(1.25f, 1.25f);
            int addr = UnitScenes.Count-1;
            foreach (PackedScene unitScene in UnitScenes) {
                Node unitType = UnitScenes[addr].Instance();
                SpawnTimerUI spawnTimerUI = unitType.GetNode<SpawnTimerUI>("SpawnTimerUI");

                spawnTimerUI.RectScale *= new Vector2(1.25f, 1.25f);
                unitType.RemoveChild(spawnTimerUI);
                spawnControlsContainer.AddChild(spawnTimerUI);
                unitType.QueueFree();
                spawnTimerUI.MyCursor = this;
                spawnTimerUI.TimerAddress = addr;
                addr--;
                SpawnTimers.Add(spawnTimerUI.GetNode<Timer>("SpawnTimer"));
            };
        } else {
            if (MySide == Unit.Side.Left) {
                Owner.GetNode<Control>("UI/VBoxContainer/MarginContainer/HBoxContainer/LSpawnTimerContainerUI").Visible = false;
            } else {
                Owner.GetNode<Control>("UI/VBoxContainer/MarginContainer/HBoxContainer/RSpawnTimerContainerUI").Visible = false;
            }
            SetSpawnTimers();
        }
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
