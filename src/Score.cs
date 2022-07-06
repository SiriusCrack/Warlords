using Godot;
using System;

public class Score : ProgressBar {
    [Signal] public delegate void Win(Unit.Side side);

    public override void _Ready() {
        Value = MaxValue/2d;
    }

    void OnLGoalEntered(Area2D area) {
        Unit unit = area.GetParent<Unit>();
        if (unit.MySide == Unit.Side.Right) {
            Value -= unit.health;
            unit.QueueFree();
            if (Value <= 0) {
                EmitSignal("Win", Unit.Side.Right);
            }
        }
    }

    void OnRGoalEntered(Area2D area) {
        Unit unit = area.GetParent<Unit>();
        if (unit.MySide == Unit.Side.Left) {
            Value += unit.health;
            unit.QueueFree();
            if (Value >= MaxValue) {
                EmitSignal("Win", Unit.Side.Left);
            }
        }
    }
}