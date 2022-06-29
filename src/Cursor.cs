using Godot;
using System;

public class Cursor : Sprite
{
    private static int[] Positions = new int[] {140, 240, 340, 440, 540, 640, 740, 840, 940};
    private PackedScene UnitScene = ResourceLoader.Load("res://Scenes/Unit.tscn") as PackedScene;
    private Vector2 Direction = Vector2.Right;
    private Timer SpawnTimer;

    private void GetInput() {
        if (Input.IsActionJustPressed("spawn")) {
            SpawnUnit();
        }
        if (Input.IsActionJustPressed("down") && Position.y < 940) {
            Position += Vector2.Down * 100;
        }
        if (Input.IsActionJustPressed("up") && Position.y > 140) {
            Position += Vector2.Up * 100;
        }
    }

    private void SpawnUnit() {
        if (SpawnTimer.IsStopped()) {
            Area2D unit = (Area2D)UnitScene.Instance();
            unit.Position = this.Position + (Direction*100);
            Owner.AddChild(unit);
            SpawnTimer.Start();
        }
    }

    private void AI() {
        if (SpawnTimer.IsStopped()) {
            Position = new Vector2 (x: Position.x, y: Positions[GD.Randi()%9]);
            SpawnUnit();
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        if (this.Position.x < 100) {
            Direction = Vector2.Left;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Direction == Vector2.Left) {
            GetInput();
        } else {
            AI();
        }
    }
}
