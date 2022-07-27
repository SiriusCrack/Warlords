using Godot;
using System;

public class Bow : Weapon {
    [Export] PackedScene Arrow;
    Unit Target;

    // Children
    Position2D Muzzle;

    public override void _Ready() {
        base._Ready();
        Muzzle = GetNode<Position2D>("Muzzle");
    }

    public void Shoot() {
        Arrow arrow = Arrow.Instance<Arrow>();
        Unit.GetParent().AddChild(arrow);
        arrow.Transform = Muzzle.GlobalTransform;
        arrow.launch();
    }
}