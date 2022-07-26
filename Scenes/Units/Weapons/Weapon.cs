using Godot;
using System;

public class Weapon : Node2D {
    // Stats
    [Export] protected int Damage;

    public void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }

    void OnBodyEntered(Unit body) {
        Hit(body);
    }
} 