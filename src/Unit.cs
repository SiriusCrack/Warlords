using Godot;
using Godot.Collections;
using System;

public class Unit : Area2D
{
	public enum Side {Left, Right};

    [Export] private int speed;
	private Timer AttackTimer;
	private Array<Unit> AttackUnits = new Array<Unit>();
	private Vector2 Direction = Vector2.Right;
	private bool Advancing = true;
	[Export] public int health;
	[Export] public int attackDamage;
	public Side MySide = Side.Left;

	public void TakeDamage(int dmg) {
		if ((health - dmg) > 0) {
			health -= dmg;
		} else {
			GetParent().RemoveChild(this);
		}
	}

	private void OnUnitAreaEntered(Unit area) {
		if (area.MySide != MySide) {
			if (AttackUnits.Count < 1) {
				Advancing = false;
				AttackTimer.Start();
			}
			AttackUnits.Add(area);
		}
	}

	private void OnUnitAreaExited(Area2D area) {
		if (AttackUnits.Contains((Unit)area)) {
			AttackUnits.Remove((Unit)area);
		}
		if (AttackUnits.Count < 1) {
			Advancing = true;
			AttackTimer.Stop();
		}
	}

	private void OnAttackTimerTimeout() {
		foreach (Unit unit in AttackUnits) {
			unit.TakeDamage(attackDamage);
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
		AttackTimer = GetNode<Timer>("AttackTimer");
		AttackTimer.WaitTime = GD.Randf() %(float)3.0;
		health = (int)(GD.Randi() %50);
		attackDamage = (int)(GD.Randi() %20);
		if (this.Position.x > 100) {
			MySide = Side.Right;
			Direction = Vector2.Left;
			this.Scale = new Vector2 (-1, 1);
		}
	}

	public override void _Process(float delta) {
		if ((Position.x < -110) || (Position.x > 2030)) {
			GetParent().RemoveChild(this);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (Advancing) {
			this.GlobalPosition += Direction * speed * delta;
		}
	}
}
