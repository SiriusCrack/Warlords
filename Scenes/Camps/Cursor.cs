using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite {
    public Camp Camp;
    public int Lane;
    public int LaneCount;

    public override void _Process(float delta) {
        if (Camp.IsPlayable) {
            GetInput();
        }
    }

    public void MoveUp() {
        if (Lane > 0) {
            Lane--;
            Position = new Vector2(Position.x, Camp.Battle.Lanes[Lane]);
        }
    }

    public void MoveDown() {
        if (Lane < LaneCount-1) {
            Lane++;
            Position = new Vector2(Position.x, Camp.Battle.Lanes[Lane]);
        }
    }

    void GetInput() {
        if (Input.IsActionJustPressed("up")) {
            MoveUp();
        }
        if (Input.IsActionJustPressed("down")) {
            MoveDown();
        }
    }
}
