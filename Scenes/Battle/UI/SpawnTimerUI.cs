using Godot;
using System;

public class SpawnTimerUI : Control {
    // Parents
    SpawnTimerContainer SpawnTimerContainer;

    int TimerAddress;
    float ProgressValue;

    // Children
    TextureProgress TextureProgress;
    Timer SpawnTimer;

    public void SetUp (
        SpawnTimerContainer spawnTimerContainer,
        int timerAddress
    ) {
        SpawnTimerContainer = spawnTimerContainer;
        TimerAddress = timerAddress;
    }


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
        SpawnTimerContainer.SpawnSelect(TimerAddress);
	}
}
