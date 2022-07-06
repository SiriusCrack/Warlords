using Godot;
using System;

public class Marine : Unit {
    [Export] Godot.Collections.Array<AudioStream> DyingSFX = new Godot.Collections.Array<AudioStream>();
    [Export] AudioStream ShootingSFX;
    [Export] Godot.Collections.Array<AudioStream> SpawningSFX = new Godot.Collections.Array<AudioStream>();

    public override void _Ready() {
        base._Ready();
        int rand = Math.Abs((int)(GD.Randi()%SpawningSFX.Count));
        AudioStreamPlayer.Stream = SpawningSFX[rand];
		AudioStreamPlayer.Play();
    }

    void StartShooting() {
        AnimationPlayer.Play("Shooting");
    }

    void HideHealth() {
        HealthBar.Visible = false;
    }

    void PlayDyingSFX() {
        int rand = Math.Abs((int)(GD.Randi()%DyingSFX.Count));
        AudioStreamPlayer.Stream = DyingSFX[rand];
        AudioStreamPlayer.Play();
    }

    void PlayShootingSFX() {
        AudioStreamPlayer.Stream = ShootingSFX;
        AudioStreamPlayer.Play();
    }
}