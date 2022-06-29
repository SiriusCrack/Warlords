using Godot;
using Godot.Collections;
using System;

public class Unit : Area2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
    [Export] public int speed;
	Array<Unit> AttackUnits = new Array<Unit>();
	Vector2 Direction = Vector2.Right;
	bool Advancing = true;

	public void OnUnitAreaEntered(Area2D area) {
		AttackUnits.Add((Unit)area);
	}

	public void OnUnitAreaExited(Area2D area) {
		if (AttackUnits.Contains((Unit)area)) {
			AttackUnits.Remove((Unit)area);
		}
	}

	public void TakeDamage(int dmg) {
		GetParent().RemoveChild(this);
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (this.Position.x > 100) {
			Direction = Vector2.Left;
			this.Scale = new Vector2 (-1, 1);
		} 
	}

	public override void _Process(float delta) {
		foreach (Unit unit in AttackUnits) {
			unit.TakeDamage(3);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		this.GlobalPosition += Direction * speed * delta;
	}
}
