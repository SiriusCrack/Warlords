using Godot;
using System;

public class Weapon : Node2D {
    [Export] int Damage;
    protected Area2D Area2D;

    public override void _Ready() {
        Area2D = GetNode<Area2D>("HitBox");
    }

    public void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }

    void OnBodyEntered(Node body) {
        Hit(body.GetParent<Unit>());
    }
}