using Godot;
using System;

public class Weapon : Node2D {
    // Stats
    [Export] protected int Damage;

    // Parents
    protected Unit Unit;

    public override void _Ready() {
        Unit = GetParent<Unit>();
    }

    public void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }

    void OnBodyEntered(Unit body) {
        Hit(body);
    }
} 