using Godot;
using System;

public class SpawnTimerUI : Control {
    public SpawnTimerContainer SpawnTimerContainer;
    public int TimerAddress;
    TextureProgress TextureProgress;
    Timer SpawnTimer;
    
    float ProgressValue;
    public override void _Ready() {
        TextureProgress = GetNode<TextureProgress>("TextureProgress");
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        TextureProgress.MaxValue = SpawnTimer.WaitTime;
        if (!OS.HasTouchscreenUiHint() && TimerAddress == 0) {
            GetNode<TextureRect>("Select").Visible = true;
        }
    }

    public override void _Process(float delta) {
        UpdateUIProgress();
    }

    public bool CheckTimer() {
        return SpawnTimer.IsStopped();
    }

    public void ResetTimer() {
        SpawnTimer.Start();
    }

    void UpdateUIProgress() {
        TextureProgress.Value = SpawnTimer.TimeLeft;
    }

	void OnTouchButtonPressed() {
        SpawnTimerContainer.Camp.SpawnUnit(TimerAddress);
	}
}
