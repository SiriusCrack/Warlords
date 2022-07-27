using Godot;
using System;

public class Arrow : Area2D {
    [Export] int Damage;
    const float MASS = 10f;
    Vector2 Velocity;
    bool launched;

    public override void _Ready() {
        SetPhysicsProcess(false);
        launched = false;
        // Velocity = Transform.x * speed;
    }

    public override void _PhysicsProcess(float delta) {
        Velocity += GravityVec * Gravity * MASS * delta;
        Position += Velocity * delta;
        // Velocity += Transform.x * speed * delta;
        // Position += Velocity;
        Rotation = Velocity.Angle();
        // Position += Vector2.Right * 10;
    }

    public void launch() {
        Velocity = Transform.x*7500;
        SetPhysicsProcess(true);
    }

    public void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }

    void OnBodyEntered(Unit body) {
        Hit(body);
        SetPhysicsProcess(false);
        Sprite sprite = GetNode<Sprite>("Sprite");
        RemoveChild(sprite);
        body.AddChild(sprite);
        sprite.GlobalTransform = GlobalTransform;
        QueueFree();
    }

    void OnTimerTimeout() {
        QueueFree();
    }
}