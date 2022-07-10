using Godot;
using Godot.Collections;
using System;

public class Unit : KinematicBody2D {
	// Stats
	[Export] public int Health;
	[Export] protected int Speed;
	[Export] protected int AttackDamage;

	public Camp Camp;
	protected Area2D Body;
	protected Area2D AttackRange;
	protected HealthBar HealthBar;
	protected AnimationPlayer AnimationPlayer;
	protected AudioStreamPlayer SFXPlayer;
	protected bool Advancing;
	protected bool Dead = false;
	protected Vector2 Direction;

	public override void _Ready() {
		Body = GetNode<Area2D>("Body");
		AttackRange = GetNode<Area2D>("AttackRange");
		HealthBar = GetNode<HealthBar>("HealthBar");
		AnimationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		SFXPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
		HealthBar.SetMaxHealth(Health);
		StartAdvancing();
	}

	public override void _PhysicsProcess(float delta) {
		if (Advancing) {
			this.Position += Direction * Speed * delta;
		}
	}

	public override void _Process(float delta) {
		int inRange = AttackRange.GetOverlappingAreas().Count;
		if (Advancing && inRange > 0) {
			StartAttacking();
		}
		if (!Advancing && inRange == 0 && !Dead) {
			StartAdvancing();
		}
	}

	public void SetDirection(Battle.Side side) {
		switch (side) {
			case Battle.Side.Left: {
				Direction = Vector2.Right;
				break;
			}
			default: {
				Direction = Vector2.Left;
				break;
			}
		}
	}

	public void SetCollision(int lane, Battle.Side side) {
		switch (side) {
			case Battle.Side.Left: {
				for (int i = 16; i < 32; i++) {
					AttackRange.SetCollisionMaskBit(i, true);
				}
				break;
			}
			default: {
				for (int i = 0; i < 16; i++) {
					AttackRange.SetCollisionMaskBit(i, true);
				}
				lane += 16;
				break;
			}
		}
		Body.SetCollisionLayerBit(lane, true);
	}

	public void Attack() {
		foreach (Area2D unit in AttackRange.GetOverlappingAreas()) {
			unit.GetParent<Unit>().TakeDamage(AttackDamage);
		}
	}

	public void TakeDamage(int damage) {
		int newHealth = Health - damage;
		if (newHealth > 0) {
			Health = newHealth;
		} else {
			Health = 0;
			StartDying();
		}
		HealthBar.UpdateHealth(Health);
	}

	void StartAdvancing() {
		Advancing = true;
		AnimationPlayer.Play("Advancing");
	}

	void StartAttacking() {
		Advancing = false;
		AnimationPlayer.Play("Attacking");
	}

	void StartDying() {
		Dead = true;
		GetNode<Area2D>("Body").QueueFree();
		AnimationPlayer.Play("Dying");
	}
}
