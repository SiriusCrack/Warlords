using Godot;
using System;

public class Archer : Unit {
    new Bow Weapon;
    Unit Target;

    public override void _Ready() {

        base._Ready();
        CircleShape2D range = new CircleShape2D();
        range.Radius = Camp.GetBattle().GetLaneStep()*10;
        Range.GetNode<CollisionShape2D>("CollisionShape2D").Shape = range;
        Weapon = (Bow)base.Weapon;
    }

    void Aim() {
        Godot.Collections.Array targetPool = Range.GetOverlappingBodies();
        if (targetPool.Count > 0) {
            Target = (Unit)targetPool[0];
            foreach (var _unit in targetPool) {
                Unit unit = (Unit)_unit;
                switch (Side) {
                    case Battle.Side.Left: if (unit.Position.x < Target.Position.x) Target = unit; break;
                    default: if (unit.Position.x > Target.Position.x) Target = unit; break;
                }
            }
        }
    }

    void Fire() {
        if (Target != null) {
            Weapon.Shoot();
        }
    }
}