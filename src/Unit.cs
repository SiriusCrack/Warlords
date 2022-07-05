using Godot;
using Godot.Collections;
using System;

public class Unit : Area2D
{
	public enum Side {Left, Right};

	public Array<Unit> AttackUnits = new Array<Unit>();
	public Timer AttackTimer;
	HealthBar HealthBar;
	public Side MySide;
	public Vector2 Direction;
	[Export] public bool Advancing = true;
    [Export] public int speed;
	[Export] public int health;
	int MaxHealth;
	[Export] public int attackDamage;

	public override void _Ready() {
		if (MySide == Side.Left) {
			Direction = Vector2.Right;
		} else {
			Direction = Vector2.Left;
			this.Scale = new Vector2 (-1, 1);
		}
        AttackTimer = GetNode<Timer>("AttackTimer");
		HealthBar = GetNode<HealthBar>("HealthBar");
		GetNode<TextureProgress>("HealthBar/Over").Value = health;
		GetNode<TextureProgress>("HealthBar/Under").Value = health;
		GetNode<TextureProgress>("HealthBar/Over").MaxValue = health;
		GetNode<TextureProgress>("HealthBar/Under").MaxValue = health;
	}

	public override void _Process(float delta) {
		// if ((Position.x < -110) || (Position.x > 2030)) {
		// 	QueueFree();
		// }
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(float delta)
	{
		if (Advancing) {
			this.GlobalPosition += Direction * speed * delta;
		}
	}
	
	public void TakeDamage(int dmg) {
		if ((health - dmg) > 0) {
			health -= dmg;
			HealthBar.UpdateHealth(health);
		} else {
			QueueFree();
		}
	}

	private void OnUnitAreaEntered(Area2D area) {
		if (((Unit)area).MySide != MySide) {
			if (AttackUnits.Count < 1) {
				Advancing = false;
				AttackTimer.Start();
			}
			AttackUnits.Add((Unit)area);
		}
	}

	private void OnUnitAreaExited(Area2D area) {
		if (AttackUnits.Contains((Unit)area)) {
			AttackUnits.Remove((Unit)area);
			if (AttackUnits.Count < 1) {
				Advancing = true;
				AttackTimer.Stop();
			}
		}
	}

	private void OnAttackTimerTimeout() {
		foreach (Unit unit in AttackUnits) {
			unit.TakeDamage(attackDamage);
		}
	}

}
