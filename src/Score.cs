using Godot;
using System;

public class Score : ProgressBar {
    public override void _Ready() {
        Value = MaxValue/2d;
    }

    void OnLGoalEntered(Area2D area) {
        Unit unit = (Unit)area;
        if (unit.MySide == Unit.Side.Right) {
            Value -= unit.health;
            unit.QueueFree();
        }
    }

    void OnRGoalEntered(Area2D area) {
        Unit unit = (Unit)area;
        if (unit.MySide == Unit.Side.Left) {
            Value += unit.health;
            unit.QueueFree();
        }
    }
}