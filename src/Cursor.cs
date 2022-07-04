using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite
{
    private static int[] Lanes = new int[] {180, 280, 380, 480, 580, 680, 780, 880, 980};
    [Export] private Array<PackedScene> UnitScenes;
    private Array<Timer> SpawnTimers = new Array<Timer>();
    [Export] private Unit.Side MySide;
    private int MyLane = 0;
    private int UnitSelect = 0;
    private Vector2 Direction;
    [Export] private bool playable;
    private Node SpawnPool;

    private void GetInput() {
        // if (Input.IsActionJustPressed("spawn1")) {
        //     SpawnUnit(0);
        // }
        // if (Input.IsActionJustPressed("spawn2")) {
        //     SpawnUnit(1);
        // }
        // if (Input.IsActionJustPressed("spawn3")) {
        //     SpawnUnit(2);
        // }
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

    private void MoveUp() {
        if (MyLane > 0) {
            MyLane--;
            Position = new Vector2(Position.x, Lanes[MyLane]);
        }
    }

    private void MoveDown() {
        if (MyLane < Lanes.Length-1) {
            MyLane++;
            Position = new Vector2(Position.x, Lanes[MyLane]);
        }
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
            unit.Position = this.Position + (Direction*75);
            SpawnTimers[arg].Start();
            int index = 0;
            foreach (Timer spawnTimer in SpawnTimers) {
                SpawnTimers[index].Start();
                index++;
            }
        }
    }

    private void AI() {
        if (SpawnTimers[UnitSelect].IsStopped()) {
            SpawnUnit(UnitSelect);
            int aiLane = (int)(GD.Randi()%9);
            while (aiLane != MyLane) {
                if (aiLane < MyLane) {
                    MoveUp();
                }
                if (aiLane > MyLane) {
                    MoveDown();
                }
            }
            AINewMove();
        }
    }

    private void AINewMove() {
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = false;
        UnitSelect = (int)(GD.Randi()%3);
        SpawnTimers[UnitSelect].GetNode<TextureRect>("../Select").Visible = true;
    }

    public override void _Ready() {
        GD.Randomize();

        SpawnPool = GetParent().GetNode("Units");

        Node spawnTimerContainerUI;
        if (MySide == Unit.Side.Left) {
            Direction = Vector2.Left;
            spawnTimerContainerUI = Owner.GetNode("UI/SpawnTimerContainerContainerUI/LSpawnTimerContainerUI");
        } else {
            Direction = Vector2.Right;
            spawnTimerContainerUI = Owner.GetNode("UI/SpawnTimerContainerContainerUI/RSpawnTimerContainerUI");
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

        if(!playable) {
            AINewMove();
        }
    }

    public override void _Process(float delta) {
        if (playable) {
            GetInput();
        } else {
            AI();
        }
    }
}
