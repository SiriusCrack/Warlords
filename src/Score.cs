using Godot;
using System;

public class Score : ProgressBar {
    Cursor RCursor;
    Cursor LCursor;

    public override void _Ready() {
        RCursor = Owner.GetNode<Cursor>("Right/RCursor");
        LCursor = Owner.GetNode<Cursor>("Left/LCursor");
        Value = MaxValue/2d;
    }

    void OnLGoalEntered(Area2D area) {
        Unit unit = area.GetParent<Unit>();
        if (unit.MySide == Unit.Side.Right) {
            Value -= unit.health;
            unit.QueueFree();
            if (Value <= 0) {
                GameOver(Unit.Side.Left);
            }
        }
    }

    void OnRGoalEntered(Area2D area) {
        Unit unit = area.GetParent<Unit>();
        if (unit.MySide == Unit.Side.Left) {
            Value += unit.health;
            unit.QueueFree();
            if (Value >= MaxValue) {
                GameOver(Unit.Side.Right);
            }
        }
    }

    void GameOver(Unit.Side loser) {
        if (
            (loser == Unit.Side.Right && LCursor.playable) ||
            (loser == Unit.Side.Left && RCursor.playable)
        ) {
            GetNode<Main>("/root/Main").Victory();
        } else {
            GetNode<Main>("/root/Main").Defeat();
        }
    }
}