using Godot;
using System;

public class Cursor : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private PackedScene UnitScene = ResourceLoader.Load("res://Scenes/Unit.tscn") as PackedScene;
    private Vector2 Direction = Vector2.Right;

    public void GetInput() {
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

    public void SpawnUnit() {
        Area2D unit = (Area2D)UnitScene.Instance();
        unit.Position = this.Position + (Direction*100);
        Owner.AddChild(unit);
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (this.Position.x < 100) {
            Direction = Vector2.Left;
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
    }
}
