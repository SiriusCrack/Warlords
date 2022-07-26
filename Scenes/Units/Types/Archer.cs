using Godot;
using System;

public class Archer : Unit {
    public override void _Ready() {
        base._Ready();
        CircleShape2D range = (CircleShape2D)Range.GetNode<CollisionShape2D>("CollisionShape2D").Shape;
        range.Radius = Camp.GetBattle().GetLaneStep();
    }
}