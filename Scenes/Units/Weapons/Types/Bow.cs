using Godot;
using System;

public class Bow : Weapon {
    Unit Target;

    new void Hit(Unit unit = null) {
        if (Target != null) {
            Target.TakeDamage(Damage);
        }
    }
}