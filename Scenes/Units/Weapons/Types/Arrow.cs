using Godot;
using System;

public class Arrow : Area2D {
    // Stats
    [Export] int Damage;
    const float SPEED = 7500;
    const float MASS = 4;
    
    Vector2 Velocity;
    bool launched;

    public override void _Ready() {
        SetPhysicsProcess(false);
        launched = false;
    }

    public override void _PhysicsProcess(float delta) {
        Velocity += GravityVec * Gravity * MASS * delta;
        Rotation = Velocity.Angle();
        Position += Velocity * delta;
    }

    public void SetCollision(Battle.Side side) {
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
				break;
		}
		for (int i = fromBit; i < toBit; i++) {
			SetCollisionMaskBit(i, true);
		}
    }

    public void Launch() {
        Velocity = Transform.x * SPEED;
        SetPhysicsProcess(true);
    }

    void OnBodyEntered(Unit body) {
        Hit(body);
        SetPhysicsProcess(false);
        Sprite sprite = GetNodeOrNull<Sprite>("Sprite");
        if (!this.IsQueuedForDeletion() && sprite != null) {
            RemoveChild(sprite);
            body.AddChild(sprite);
            sprite.GlobalTransform = GlobalTransform;
            QueueFree();
        }
    }

    void OnTimerTimeout() {
        QueueFree();
    }

    void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }
}