using Godot;
using Godot.Collections;
using System;

public class Cursor : Sprite {
    // Parents
    Battle Battle;
    Camp Camp;

    bool IsPlayable;
    int LaneCount;
    int Lane;

    public void SetUp (
        Battle battle,
        Camp camp,
        bool isPlayable
    ) {
        Battle = battle;
        Camp = camp;
        IsPlayable = isPlayable;
        LaneCount = Battle.Lanes.Count;
        Lane = 0;
        Position = new Vector2(Position.x, Battle.Lanes[0]);
    }

    public override void _Process(float delta) {
        if (IsPlayable) {
            CheckInput();
        }
    }

    public void MoveUp() {
        if (Lane > 0) {
            Lane--;
            Position = new Vector2(Position.x, Battle.Lanes[Lane]);
        }
    }

    public void MoveDown() {
        if (Lane < LaneCount-1) {
            Lane++;
            Position = new Vector2(Position.x, Battle.Lanes[Lane]);
        }
    }

    public int GetLane() {
        return Lane;
    }

    void CheckInput() {
        if (Input.IsActionJustPressed("up")) {
            MoveUp();
        }
        if (Input.IsActionJustPressed("down")) {
            MoveDown();
        }
    }
}
