using Godot;
using System;

public class SpawnTimerUI : Control {
    TextureProgress TextureProgress;
    Timer SpawnTimer;
    // Tween tween;
    float ProgressValue;
    public override void _Ready() {
        TextureProgress = GetNode<TextureProgress>("TextureProgress");
        SpawnTimer = GetNode<Timer>("SpawnTimer");
        // tween = GetNode<Tween>("TextureProgress/Tween");
        // tween.InterpolateProperty(TextureProgress, TextureProgress.Value, TextureProgress.Value)
        TextureProgress.MaxValue = SpawnTimer.WaitTime;
    }

    public override void _Process(float delta) {
        TextureProgress.Value = SpawnTimer.TimeLeft;
    }
}
