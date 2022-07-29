using Godot;
using System;

public class Bow : Weapon {
    [Export] PackedScene Arrow;
    Unit Target;
    float RangeBounds;

    // Children
    Position2D Muzzle;

    public override void _Ready() {
        base._Ready();
        Muzzle = GetNode<Position2D>("Muzzle");
    }

    public void SetRangeBounds () {
        CapsuleShape2D range = base.Range.GetNode<CollisionShape2D>("CollisionShape2D").Shape as CapsuleShape2D;
        RangeBounds = range.Radius;
    }

    public void Shoot() {
        Arrow arrow = Arrow.Instance<Arrow>();
        Unit.GetParent().AddChild(arrow);
        arrow.Transform = Muzzle.GlobalTransform;
        arrow.SetCollision(Unit.GetSide());
        arrow.Launch();
    }
}