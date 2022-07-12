using Godot;
using Godot.Collections;
using System;

public class Unit : Node2D {
	// Stats
	[Export] int Health;
	[Export] int Speed;

	// Parents
	Camp Camp;
	Cursor Cursor;

	Battle.Side Side;
	Vector2 Direction;
	float SpawnPoint;
	bool Dead;
	bool Advancing;

	// Children
	AudioStreamPlayer SFXPlayer;
	AnimationPlayer BodyAnimationPlayer;
	KinematicBody2D Body;
	Weapon Weapon;
	Area2D Range;
	HealthBar HealthBar;

    public void SetUp (
		Camp camp,
		Cursor cursor,
		Battle.Side side,
		float spawnPoint
	) {
		Camp = camp;
		Cursor = cursor;
		Side = side;
		SetSpawnPoint(spawnPoint);
		SetDirection(Side);
		Dead = false;
    }

	public override void _Ready() {
		SFXPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
		BodyAnimationPlayer = GetNode<AnimationPlayer>("BodyAnimationPlayer");
		Body = GetNode<KinematicBody2D>("Body");
		Weapon = GetNodeOrNull<Weapon>("Weapon");
		if (Weapon != null) SetWeapon();
		SetCollision(Side);
		HealthBar = GetNode<HealthBar>("HealthBar");
		HealthBar.SetMaxHealth(Health);
		Scale = new Vector2(Scale.x * Direction.x, Scale.y);
		GlobalPosition = new Vector2(Cursor.GlobalPosition.x + SpawnPoint, Cursor.GlobalPosition.y);
		StartAdvancing();
	}

	public override void _PhysicsProcess(float delta) {
		if (Advancing) {
			Position += Direction * Speed * delta;
		}
	}

	public override void _Process(float delta) {
		int inRange = Range.GetOverlappingBodies().Count;
		if (Advancing && inRange > 0) {
			StartAttacking();
		}
		if (!Advancing && inRange == 0 && !Dead) {
			StartAdvancing();
		}
	}

	public int GetHealth() {
		return Health;
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

	void SetDirection(Battle.Side side) {
		switch (side) {
			case Battle.Side.Left:
				Direction = Vector2.Right;
				break;
			default:
				Direction = Vector2.Left;
				break;
		}
	}

	void SetCollision(Battle.Side side) {
		int lane = Cursor.GetLane();
		int fromBit;
		int toBit;
		switch (side) {
			case Battle.Side.Left:
				fromBit = 16;
				toBit = 32;
				break;
			default:
				fromBit = 0;
				toBit = 16;
				lane += 16;
				break;
		}
		for (int i = fromBit; i < toBit; i++) {
			Weapon.GetNode<Area2D>("HitBox").SetCollisionMaskBit(i, true);
			Range.SetCollisionMaskBit(i, true);
		}
		Body.SetCollisionLayerBit(lane, true);
	}

	void SetSpawnPoint(float spawnPoint) {
		SpawnPoint = spawnPoint * Direction.x;
	}

	void SetWeapon() {
		Range = Weapon.GetNode<Area2D>("Range");
		Weapon.RemoveChild(Range);
		AddChildBelowNode(Weapon, Range);
	}

	void StartAdvancing() {
		Advancing = true;
		// AnimationPlayer.Play("Advancing");
	}

	void StartAttacking() {
		Advancing = false;
		// AnimationPlayer.Play("Attack");
	}

	void StartDying() {
		Dead = true;
		Body.QueueFree();
		// AnimationPlayer.Play("Dying");
		QueueFree();
	}
}
