using Godot;
using Godot.Collections;
using System;

public class Unit : KinematicBody2D
{
	public enum Side {Left, Right};

	int enemies;
	Timer AttackTimer;
	protected HealthBar HealthBar;
	protected AnimationPlayer AnimationPlayer;
	protected AudioStreamPlayer AudioStreamPlayer;
	Area2D AttackRange;
	public Side MySide;
	public Vector2 Direction;
	[Export] bool Advancing = true;
    [Export] int speed;
	[Export] public int health;
	int MaxHealth;
	[Export] int attackDamage;
	bool Dead = false;

	public override void _Ready() {
		GD.Randomize();
		if (MySide == Side.Left) {
			Direction = Vector2.Right;
		} else {
			Direction = Vector2.Left;
			this.Scale = new Vector2 (-1, 1);
		}
        AttackTimer = GetNode<Timer>("AttackTimer");
		HealthBar = GetNode<HealthBar>("HealthBar");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		AudioStreamPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		AttackRange = GetNode<Area2D>("Attack");
		GetNode<TextureProgress>("HealthBar/Over").Value = health;
		GetNode<TextureProgress>("HealthBar/Under").Value = health;
		GetNode<TextureProgress>("HealthBar/Over").MaxValue = health;
		GetNode<TextureProgress>("HealthBar/Under").MaxValue = health;
		AnimationPlayer.Play("Walking");
	}

	public override void _PhysicsProcess(float delta) {
		if (Advancing) {
			this.GlobalPosition += Direction * speed * delta;
		}
	}

	public override void _Process(float delta) {
		if (!Dead) {
			if (Advancing == false) {
				enemies = 0;
				foreach (Area2D collision in AttackRange.GetOverlappingAreas()) {
					if (collision.GetParent<Unit>().MySide != MySide) {
						enemies++;
					}
				}
				if (enemies == 0) {
					Advancing = true;
					AttackTimer.Stop();
					AnimationPlayer.Play("Walking");
				}
			}
		}
	}

	void Attack() {
		foreach (Area2D unit in AttackRange.GetOverlappingAreas()) {
			if(unit.GetParent<Unit>().MySide != MySide) {
				unit.GetParent<Unit>().TakeDamage(attackDamage);
			}
		}
	}

	void Die() {
		Dead = true;
		GetNode<Area2D>("Body").QueueFree();
	}
	
	public void TakeDamage(int dmg) {
		if ((health - dmg) > 0) {
			health -= dmg;
			HealthBar.UpdateHealth(health);
		} else {
			health = 0;
			HealthBar.UpdateHealth(health);
			AnimationPlayer.Play("Dying");
		}
	}

	private void OnUnitAreaEntered(Area2D area) {
		if (Advancing == true && area.GetParent<Unit>().MySide != MySide) {
			Advancing = false;
			AttackTimer.Start();
			AnimationPlayer.Play("Attacking");
		}
	}

	// private void OnUnitAreaExited(Area2D area) {
	// 	int enemies = 0;
	// 	foreach (Area2D collision in AttackRange.GetOverlappingAreas()) {
	// 		if (collision.GetParent<Unit>().MySide != MySide) {
	// 			enemies++;
	// 			Advancing = false;
	// 		}
	// 	}
	// 	GD.Print(enemies, Advancing);
	// 	if (Advancing == false && enemies == 0) {
	// 		Advancing = true;
	// 		AttackTimer.Stop();
	// 		AnimationPlayer.Play("Walking");
	// 	}
	// }

	private void OnAttackTimerTimeout() {
		Attack();
	}
}
