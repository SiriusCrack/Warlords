using Godot;
using System;

public class TouchInput : Control {
    public override void _Ready() {
        int projectWidth = (int)ProjectSettings.GetSetting("display/window/size/width");
        int projectHeight = (int)ProjectSettings.GetSetting("display/window/size/height");
        float viewportWidth = GetViewportRect().Size.x;
        float viewportHeight = GetViewportRect().Size.y;
        GetNode<Control>("Lanes").RectScale *= viewportHeight / (float)projectHeight;
        GetNode<Control>("SpawnControls").RectScale *= viewportWidth / (float)projectWidth;
    }
}