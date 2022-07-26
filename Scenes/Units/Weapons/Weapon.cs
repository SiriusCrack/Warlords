using Godot;
using System;

public class Weapon : Node2D {
    // Stats
    [Export] int Damage;

    // Children
    AnimationPlayer WeaponAnimationPlayer;

    public override void _Ready() {
        WeaponAnimationPlayer = GetNode<AnimationPlayer>("WeaponAnimationPlayer");
    }

    public void Attack() {
        WeaponAnimationPlayer.Play("Attack");
    }

    void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }

    void OnBodyEntered(Unit body) {
        Hit(body);
    }
} 