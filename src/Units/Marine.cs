using Godot;
using System;

public class Marine : Unit {
    void StartShooting() {
        AnimationPlayer.Play("Shooting");
    }

    void HideHealth() {
        HealthBar.Visible = false;
    }
}