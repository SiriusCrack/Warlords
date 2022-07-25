using Godot;
using System;

public class UI : Control {
    // Parents
    Battle Battle;

    // Children
    VBoxContainer VBoxContainer;
    ProgressBar Score;
    SpawnTimerContainer LeftSpawnTimerContainer;
    SpawnTimerContainer RightSpawnTimerContainer;

    public void SetUp(Battle battle) {
        Battle = battle;
    }

    public override void _Ready() {
        VBoxContainer = GetNode<VBoxContainer>("VBoxContainer");
        Score = VBoxContainer.GetNode<ProgressBar>("Score");
        LeftSpawnTimerContainer = VBoxContainer.GetNode<SpawnTimerContainer>("MarginContainer/HBoxContainer/SpawnTimerContainerLeft");
        RightSpawnTimerContainer = VBoxContainer.GetNode<SpawnTimerContainer>("MarginContainer/HBoxContainer/SpawnTimerContainerRight");
        SetScore();
    }

    public SpawnTimerContainer GetSpawnTimerContainer(Battle.Side side, bool isPlayable) {
        switch (side) {
            case Battle.Side.Left: return LeftSpawnTimerContainer;
            default: return RightSpawnTimerContainer;
        }
    }

    public float GetUIDepth() {
        return VBoxContainer.RectSize.y;
    }

    public void OnGoalEntered(Unit unit, Battle.Side side) {
        double newValue = Score.Value;
        switch (side){
            case Battle.Side.Left: newValue -= unit.GetHealth(); break;
            default: newValue += unit.GetHealth(); break;
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
