using Godot;
using System;

public class SpawnTimerUI : Control {
    TextureProgress TextureProgress;
    Timer SpawnTimer;
    public Cursor MyCursor;
    public int TimerAddress;
    public Unit.Side MySide;
    
    float ProgressValue;
    public override void _Ready() {
        TextureProgress = GetNode<TextureProgress>("TextureProgress");
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        TextureProgress.MaxValue = SpawnTimer.WaitTime;
    }

    public override void _Process(float delta) {
        TextureProgress.Value = SpawnTimer.TimeLeft;
    }

	void OnTouchButtonPressed() {
        GD.Print("nice");
		MyCursor.SpawnUnit(TimerAddress);
	}
}
