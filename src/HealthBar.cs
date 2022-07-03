using Godot;
using System;

public class HealthBar : Control {
    TextureProgress UnderBar;
    TextureProgress OverBar;
    Tween Tween;

    public override void _Ready() {
        UnderBar = GetNode<TextureProgress>("Under");
        OverBar = GetNode<TextureProgress>("Over");
        Tween = GetNode<Tween>("Tween");
    }

    public void UpdateHealth(int health) {
        if (health < OverBar.MaxValue) {
            Visible = true;
        }
        OverBar.Value = health;
        Tween.InterpolateProperty(UnderBar, "value", UnderBar.Value, health, 0.5f, Tween.TransitionType.Sine, Tween.EaseType.InOut);
        Tween.Start();
    }
}
