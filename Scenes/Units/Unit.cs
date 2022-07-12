using Godot;
using Godot.Collections;
using System;

public class Unit : Node2D {
	// Stats
	[Export] public int Health;
	[Export] int Speed;
	[Export] int AttackDamage;

	public Camp Camp;
	KinematicBody2D Body;
	HealthBar HealthBar;
	AnimationPlayer AnimationPlayer;
	AudioStreamPlayer SFXPlayer;
	bool Advancing;
	bool Dead = false;
	Vector2 Direction;
	Weapon Weapon;

	public override void _Ready() {
		Body = GetNode<KinematicBody2D>("Body");
		HealthBar = GetNode<HealthBar>("HealthBar");
		SFXPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
		Weapon = GetNode<Weapon>("Sword");
		AnimationPlayer = Weapon.GetNode<AnimationPlayer>("AnimationPlayer");
		HealthBar.SetMaxHealth(Health);
		StartAdvancing();
	}

	public override void _PhysicsProcess(float delta) {
		if (Advancing) {
			this.Position += Direction * Speed * delta;
		}
	}

	public override void _Process(float delta) {
		int inRange = GetNode<Area2D>("Range").GetOverlappingBodies().Count;
		GD.Print(inRange);
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
				Scale = new Vector2(Scale.x*-1, Scale.y);
				break;
			}
		}
	}

	public void SetCollision(int lane, Battle.Side side) {
		switch (side) {
			case Battle.Side.Left: {
				for (int i = 16; i < 32; i++) {
					Weapon.GetNode<Area2D>("HitBox").SetCollisionMaskBit(i, true);
					GetNode<Area2D>("Range").SetCollisionMaskBit(i, true);
				}
				break;
			}
			default: {
				for (int i = 0; i < 16; i++) {
					Weapon.GetNode<Area2D>("HitBox").SetCollisionMaskBit(i, true);
					GetNode<Area2D>("Range").SetCollisionMaskBit(i, true);
				}
				lane += 16;
				break;
			}
		}
		Body.SetCollisionLayerBit(lane, true);
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
		// AnimationPlayer.Play("Advancing");
	}

	void StartAttacking() {
		Advancing = false;
		AnimationPlayer.Play("Attack");

	}

	void StartDying() {
		Dead = true;
		// GetNode<KinematicBody2D>("Body").QueueFree();
		QueueFree();
	}
}
