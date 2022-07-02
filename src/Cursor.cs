using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite
{
    private static int[] Lanes = new int[] {140, 240, 340, 440, 540, 640, 740, 840, 940};
    [Export] private Array<PackedScene> UnitScenes;
    private Array<Timer> SpawnTimers = new Array<Timer>();
    [Export] private Unit.Side MySide;
    private int MyLane = 0;
    private Vector2 Direction;
    [Export] private bool playable;
    private int AIMove;

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
            Unit unit = UnitScenes[arg].Instance<Unit>();
            unit.GetNode("SpawnTimer").QueueFree();
            unit.MySide = MySide;
            if (MySide == Unit.Side.Left) {
                unit.Direction = Vector2.Right;
            } else {
                unit.Direction = Vector2.Left;
                unit.Scale = new Vector2 (-1, 1);
            }
            unit.Position = this.Position + (Direction*75);
            Owner.AddChild(unit);
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

        if (MySide == Unit.Side.Left) {
            Direction = Vector2.Left;
        } else {
            Direction = Vector2.Right;
        }
        
        Node spawnTimersGroup = new Node();
        spawnTimersGroup.Name = "SpawnTimersGroup";
        AddChild(spawnTimersGroup);
        foreach (PackedScene unitScene in UnitScenes) {
            Unit unit = unitScene.Instance<Unit>();
            unit.GetNode("Sprite").QueueFree();
            unit.GetNode("CollisionShape2D").QueueFree();
            unit.GetNode("AttackTimer").QueueFree();
            unit.Monitoring = false;
            unit.Monitorable = false;
            unit.MySide = MySide;
            unit.Advancing = false;
            unit.GlobalPosition = new Vector2(33,-33);
            spawnTimersGroup.AddChild(unit);
            SpawnTimers.Add(unit.GetNode<Timer>("SpawnTimer"));
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
