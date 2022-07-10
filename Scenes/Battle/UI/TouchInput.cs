using Godot;
using System;

public class TouchInput : UI {
    ProgressBar Score;

    public override void _Ready() {
        Score = GetNode<ProgressBar>("Score");
        ScaleUI();
    }

    public new SpawnTimerContainer GetSpawnTimerContainer(Battle.Side side, bool isPlayable) {
        SpawnTimerContainer spawnTimerContainer;
        if (isPlayable) {
            spawnTimerContainer = GetNode<SpawnTimerContainer>("SpawnControls/SpawnTimerContainer");
        } else {
            spawnTimerContainer = GetNode<SpawnTimerContainer>("SpawnTimerContainerAI");
        }
        return spawnTimerContainer;
    }

    public new float GetUIDepth() {
        return GetNode<TextureProgress>("Score").RectSize.y;
    }

    void ScaleUI() {
        int projectWidth = (int)ProjectSettings.GetSetting("display/window/size/width");
        int projectHeight = (int)ProjectSettings.GetSetting("display/window/size/height");
        float viewportWidth = GetViewportRect().Size.x;
        float viewportHeight = GetViewportRect().Size.y;
        GetNode<Control>("Lanes").RectScale *= viewportHeight / (float)projectHeight;
        GetNode<Control>("SpawnControls").RectScale *= viewportWidth / (float)projectWidth;
    }
}