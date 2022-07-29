using Godot;
using System;

public class Weapon : Node2D {
    // Stats
    [Export] protected int Damage;

    // Parents
    protected Unit Unit;

    // Children
    protected Area2D Range;

    public override void _Ready() {
        Unit = GetParent().GetParent<Unit>();
        Range = Unit.GetNode<Area2D>("Range");
        SetCollision(Unit.GetSide());
    }

    public void SetCollision(Battle.Side side) {
        int fromBit;
		int toBit;
		switch (side) {
			case Battle.Side.Left:
				fromBit = 16;
				toBit = 32;
				break;
			default:
				fromBit = 0;
				toBit = 16;
				break;
		}
        Area2D hitbox = GetNode<Area2D>("HitBox");
		for (int i = fromBit; i < toBit; i++) {
			hitbox.SetCollisionMaskBit(i, true);
			Range.SetCollisionMaskBit(i, true);
		}
    }

    void Hit(Unit unit) {
        unit.TakeDamage(Damage);
    }

    void OnBodyEntered(Unit body) {
        Hit(body);
    }
} 