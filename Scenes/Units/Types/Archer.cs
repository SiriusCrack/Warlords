using Godot;
using System;

public class Archer : Unit {
    // Stats
    [Export] float RangeDistance;

    Unit Target;
    float HandRestAngle;
    bool Aiming;

    // Children
    new Bow Weapon;
    Tween Tween;
    

    public override void _Ready() {
        base._Ready();
        Weapon = base.Weapon as Bow;
        HandRestAngle = Hand.Rotation;
        SetRange();
        Weapon.SetRangeBounds();
        Tween = GetNode<Tween>("Tween");
        Aiming = false;
    }

    public override void _Process(float delta) {
        base._Process(delta);
        if (Aiming) {
            Hand.Rotation = Aim();
        }
    }

    void SetRange() {
        CapsuleShape2D range = new CapsuleShape2D();
        range.Radius = Camp.GetBattle().GetLaneStep()*10;
        range.Height = RangeDistance;
        Range.GetNode<CollisionShape2D>("CollisionShape2D").Shape = range;
    }

    float Aim() {
        if (IsInstanceValid(Target)) {
            float newAngle = Position.AngleToPoint(Target.GlobalPosition);
            if (Side == Battle.Side.Left) {
                if (newAngle > 0) {
                    newAngle = (float)Math.PI - newAngle;
                } else {
                    newAngle = (float)(-1 * Math.PI) - newAngle;
                }
            }
            newAngle *= -1;
            return newAngle;
        } else {
            return Hand.Rotation;
        }
    }

    void PickTarget() {
        Aiming = true;
        Godot.Collections.Array targetPool = Range.GetOverlappingBodies();
        if (targetPool.Count > 0) {
            Target = targetPool[0] as Unit;
            foreach (var _unit in targetPool) {
                Unit unit = _unit as Unit;
                switch (Side) {
                    case Battle.Side.Left: if (unit.Position.x < Target.Position.x) Target = unit; break;
                    default: if (unit.Position.x > Target.Position.x) Target = unit; break;
                }
            }
            Tween.InterpolateProperty(Hand, "rotation", Hand.Rotation, Aim(), 0.7f, Tween.TransitionType.Cubic, Tween.EaseType.Out);
            Tween.Start();
        }
    }

    void Fire() {
        Aiming = false;
        if (Target != null) {
            Weapon.Shoot();
        }
        Tween.InterpolateProperty(Hand, "rotation", Hand.Rotation, HandRestAngle, 0.3f, Tween.TransitionType.Cubic, Tween.EaseType.Out);
        Tween.Start();
    }
}