using Godot;
using Godot.Collections;
using System;

public class Unit : KinematicBody2D {
	// Stats
	[Export] int Health;
	[Export] int Speed;

	// Parents
	protected Camp Camp;
	Cursor Cursor;

	protected Battle.Side Side;
	Vector2 Direction;
	float SpawnPoint;
	bool Dead;
	bool Advancing;
	int Lane;

	// Children
	AudioStreamPlayer SFXPlayer;
	AnimationPlayer BodyAnimationPlayer;
	protected Node2D Hand;
	protected Weapon Weapon;
	protected Area2D Range;
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
		Hand =  GetNode<Node2D>("Hand");
		Weapon = Hand.GetNode<Weapon>("Weapon");
		Range = GetNode<Area2D>("Range");
		SetCollision();
		HealthBar = GetNode<HealthBar>("HealthBar");
		HealthBar.SetMaxHealth(Health);
		Scale = new Vector2(Scale.x * Direction.x, Scale.y);
		GlobalPosition = new Vector2(Cursor.GlobalPosition.x - SpawnPoint, Cursor.GlobalPosition.y);
		StartAdvancing();
	}

	public override void _PhysicsProcess(float delta) {
		Vector2 relVec = Vector2.Right * Direction * Speed * delta;
		var collisionCheck = MoveAndCollide(relVec, true, true, true);
		if (collisionCheck != null) {
			SetCollisionMaskBit(Lane, true);
		}
		if (Advancing && collisionCheck == null) {
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
		if (!Advancing && inRange > 0 && !Dead && !BodyAnimationPlayer.IsPlaying()) {
			BodyAnimationPlayer.Play("Attacking");
		}
	}

	public int GetHealth() {
		return Health;
	}

	public Battle.Side GetSide() {
		return Side;
	}

	public Area2D GetRange() {
		return Range;
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

	void SetCollision() {
		Lane = Cursor.GetLane();
		if (Side == Battle.Side.Right) {
			Lane += 16;
		}
		SetCollisionLayerBit(Lane, true);
	}

	void SetSpawnPoint(float spawnPoint) {
		SpawnPoint = spawnPoint * Direction.x;
	}

	void StartAdvancing() {
		Advancing = true;
		SetCollisionMaskBit(Lane, false);
	}

	void StartAttacking() {
		Advancing = false;
		SetCollisionMaskBit(Lane, true);
		BodyAnimationPlayer.Play("Attacking");
	}

	void StartDying() {
		Dead = true;
		Advancing = false;
		for (int i = 0; i < 32; i++) {
			SetCollisionMaskBit(i, false);
			SetCollisionLayerBit(i, false);
		}
		// GetNode("CollisionShape2D").QueueFree();
		BodyAnimationPlayer.Play("Dying");
	}
}
