using Godot;
using Godot.Collections;
using System;

public class Unit : KinematicBody2D {
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
	int Lane;

	// Children
	AudioStreamPlayer SFXPlayer;
	AnimationPlayer BodyAnimationPlayer;
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
		SetDirection(Side);
		SetSpawnPoint(spawnPoint);
		Dead = false;
    }

	public override void _Ready() {
		SFXPlayer = GetNode<AudioStreamPlayer>("SFXPlayer");
		BodyAnimationPlayer = GetNode<AnimationPlayer>("BodyAnimationPlayer");
		Weapon = GetNode<Weapon>("Hand/Weapon");
		Range = GetNode<Area2D>("Range");
		SetCollision(Side);
		HealthBar = GetNode<HealthBar>("HealthBar");
		HealthBar.SetMaxHealth(Health);
		Scale = new Vector2(Scale.x * Direction.x, Scale.y);
		GlobalPosition = new Vector2(Cursor.GlobalPosition.x - SpawnPoint, Cursor.GlobalPosition.y);
		StartAdvancing();
	}

	public override void _PhysicsProcess(float delta) {
		if (Advancing) {
			MoveAndCollide(Vector2.Right * Direction * Speed * delta);
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

	void Attack() {
		Weapon.Attack();
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
		Lane = Cursor.GetLane();
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
				Lane += 16;
				break;
		}
		for (int i = fromBit; i < toBit; i++) {
			Weapon.GetNode<Area2D>("HitBox").SetCollisionMaskBit(i, true);
			Range.SetCollisionMaskBit(i, true);
		}
		SetCollisionLayerBit(Lane, true);
	}

	void SetSpawnPoint(float spawnPoint) {
		SpawnPoint = spawnPoint * Direction.x;
	}

	void StartAdvancing() {
		Advancing = true;
		SetCollisionMaskBit(Lane, false);
		BodyAnimationPlayer.Play("Advancing");
	}

	void StartAttacking() {
		Advancing = false;
		SetCollisionMaskBit(Lane, true);
		BodyAnimationPlayer.Play("Attacking");
	}

	void StartDying() {
		Dead = true;
		GetNode("CollisionShape2D").QueueFree();
		BodyAnimationPlayer.Play("Dying");
		QueueFree();
	}
}
