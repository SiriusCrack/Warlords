using Godot;
using System;

public class Cursor : Sprite
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private PackedScene UnitScene = ResourceLoader.Load("res://Scenes/Unit.tscn") as PackedScene;

    public void GetInput() {
        if (Input.IsActionJustPressed("spawn")) {
            SpawnUnit();
            GD.Print("spawned");
        }
        if (Input.IsActionJustPressed("down") && Position.y < 940) {
            Position += Vector2.Down * 100;
        }
        if (Input.IsActionJustPressed("up") && Position.y > 140) {
            Position += Vector2.Up * 100;
        }
    }

    public void SpawnUnit() {
        RigidBody2D unit = (RigidBody2D)UnitScene.Instance();
        Owner.AddChild(unit);
        unit.Position = this.Position;
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        GetInput();
    }
}
