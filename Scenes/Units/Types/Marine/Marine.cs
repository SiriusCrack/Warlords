using Godot;
using System;

public class Marine : Unit {
    [Export] Godot.Collections.Array<AudioStream> DyingSFX = new Godot.Collections.Array<AudioStream>();
    [Export] AudioStream ShootingSFX;
    [Export] Godot.Collections.Array<AudioStream> SpawningSFX = new Godot.Collections.Array<AudioStream>();

    public override void _Ready() {
        base._Ready();
        SetLaneRange();
        int rand = Math.Abs((int)(GD.Randi()%SpawningSFX.Count));
        SFXPlayer.Stream = SpawningSFX[rand];
		SFXPlayer.Play();
    }

	public new void Attack() {
        if (AttackRange.GetOverlappingAreas().Count > 0) {
            Area2D target = (Area2D)AttackRange.GetOverlappingAreas()[0];
            target.GetParent<Unit>().TakeDamage(AttackDamage);
        }
	}

    void SetLaneRange() {
        AttackRange.GetNode<CollisionPolygon2D>("CollisionPolygon2D").Polygon[1] = new Vector2(100, Camp.Battle.LaneStep*2);
        AttackRange.GetNode<CollisionPolygon2D>("CollisionPolygon2D").Polygon[2] = new Vector2(100, -Camp.Battle.LaneStep*2);
        GD.Print(Camp.Battle.LaneStep*2);
    }

    void StartShooting() {
        AnimationPlayer.Play("Shooting");
    }

    void HideHealth() {
        HealthBar.Visible = false;
    }

    void PlayDyingSFX() {
        int rand = Math.Abs((int)(GD.Randi()%DyingSFX.Count));
        SFXPlayer.Stream = DyingSFX[rand];
        SFXPlayer.Play();
    }

    void PlayShootingSFX() {
        SFXPlayer.Stream = ShootingSFX;
        SFXPlayer.Play();
    }
}