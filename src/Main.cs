using Godot;
using System;

public class Main : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] public PackedScene UnitScene;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        RigidBody2D unit = (RigidBody2D)UnitScene.Instance();
        Vector2 unitSpawn;
        unitSpawn.x = 10;
        unitSpawn.y = 10;
        unit.Position = unitSpawn;
        AddChild(unit);
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
