using Godot;
using System;

public class UI : Control {
    ProgressBar Score;

    public override void _Ready() {
        Score = GetNode<ProgressBar>("VBoxContainer/Score");
        SetScore();
    }

    public SpawnTimerContainer GetSpawnTimerContainer(Battle.Side side, bool isPlayable) {
        SpawnTimerContainer spawnTimerContainer;
        switch (side) {
            case Battle.Side.Left: {
                spawnTimerContainer = GetNode<SpawnTimerContainer>("VBoxContainer/MarginContainer/HBoxContainer/SpawnTimerContainerLeft");
                break;
            }
            default: {
                spawnTimerContainer = GetNode<SpawnTimerContainer>("VBoxContainer/MarginContainer/HBoxContainer/SpawnTimerContainerRight");
                break;
            }
        }
        return spawnTimerContainer;
    }

    public float GetUIDepth() {
        return GetNode<VBoxContainer>("VBoxContainer").RectSize.y;
    }

    public void OnGoalEntered(Area2D area) {
        Unit unit = area.GetParent<Unit>();
        double newValue = Score.Value;
        switch (unit.Camp.Side){
            case Battle.Side.Left: {
                newValue += unit.Health;
                break;
            }
            default: {
                newValue -= unit.Health;
                break;
            }
        }
        unit.QueueFree();
        if (newValue > Score.MaxValue) {
            Score.Value = Score.MaxValue;
            GameOver(Battle.Side.Left);
        } else if (newValue < 0) {
            Score.Value = 0;
            GameOver(Battle.Side.Right);
        } else {
            Score.Value = newValue;
        }
    }

    void SetScore() {
        Score.MaxValue = 1500;
        Score.Value = Score.MaxValue/2;
    }

    void GameOver(Battle.Side winner) {
        GetParent<Battle>().GameOver(winner);
    }
}
