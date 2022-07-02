using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite
{
    private static int[] Lanes = new int[] {140, 240, 340, 440, 540, 640, 740, 840, 940};
    [Export] private Array<PackedScene> UnitScenes;
    private Array<Timer> SpawnTimers = new Array<Timer>();
    private Unit.Side MySide;
    private int MyLane = 0;
    private Vector2 Direction;
    [Export] private bool playable;
    private int AIMove;
    private Node SpawnPool;

    private void GetInput() {
        if (Input.IsActionJustPressed("spawn1")) {
            SpawnUnit(0);
        }
        if (Input.IsActionJustPressed("spawn2")) {
            SpawnUnit(1);
        }
        if (Input.IsActionJustPressed("spawn3")) {
            SpawnUnit(2);
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
            unit.Direction = Direction;
            if (MySide == Unit.Side.Right) {
                unit.Scale = new Vector2 (-1, 1);
            }
            unit.Position = this.Position - (Direction*100);
            SpawnTimers[arg].Start();
            int index = 0;
            foreach (Timer spawnTimer in SpawnTimers) {
                SpawnTimers[index].Start();
                index++;
            }
        }
    }

    private void AI() {
        if (SpawnTimers[AIMove].IsStopped()) {
            SpawnUnit(AIMove);
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
        AIMove = (int)(GD.Randi()%3);
    }

    public override void _Ready() {
        GD.Randomize();

        SpawnPool = GetParent().GetNode("Units");

        GD.Print(GetParent().Name);
        if (GetParent().Name == "Left") {
            MySide = Unit.Side.Left;
            Direction = Vector2.Right;
        } else {
            MySide = Unit.Side.Right;
            Direction = Vector2.Left;
        }
        
        Node spawnTimersGroup = new Node();
        spawnTimersGroup.Name = "SpawnTimersGroup";
        AddChild(spawnTimersGroup);
        foreach (PackedScene unitScene in UnitScenes) {
            Node unitType = unitScene.Instance();
            Timer spawnTimer = unitType.GetNode<Timer>("SpawnTimer");
            unitType.RemoveChild(spawnTimer);
            spawnTimersGroup.AddChild(spawnTimer);
            unitType.QueueFree();
            SpawnTimers.Add(spawnTimer);
        }

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
