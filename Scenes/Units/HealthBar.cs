using Godot;
using System;

public class HealthBar : Control {
    TextureProgress UnderBar;
    TextureProgress OverBar;
    // SceneTreeTween Tween;

    public override void _Ready() {
        UnderBar = GetNode<TextureProgress>("Under");
        OverBar = GetNode<TextureProgress>("Over");
    }

    public void SetMaxHealth(int health) {
		UnderBar.Value = health;
		OverBar.Value = health;
		UnderBar.MaxValue = health;
		OverBar.MaxValue = health;
    }

    public void UpdateHealth(int health) {
        if (health < OverBar.MaxValue) {
            Visible = true;
        }
        OverBar.Value = health;
        SceneTreeTween tween = CreateTween();
        tween.TweenProperty(UnderBar, "value", (float)health, 0.5f).SetEase(Godot.Tween.EaseType.InOut);
        // Tween.InterpolateProperty(UnderBar, "value", UnderBar.Value, health, 0.5f, Tween.TransitionType.Sine, Tween.EaseType.InOut);
        // Tween.Start();
    }
}
