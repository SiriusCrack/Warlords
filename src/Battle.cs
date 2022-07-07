using Godot;
using Godot.Collections;
using System;

public class Battle : Node {
    [Export] PackedScene Victory;
    [Export] PackedScene Defeat;
    public Array<float> Lanes = new Array<float>();
    public float UIDepth;
    Cursor LCursor;
    Cursor RCursor;

    public override void _Ready() {
        LCursor = GetNode<Cursor>("Left/LCursor");
        RCursor = GetNode<Cursor>("Right/RCursor");
        LCursor.SetUp();
        RCursor.SetUp();
        SetUIDepth();
        SetLanes();
    }
    
    void OnWin(Unit.Side side) {
        if (side == Unit.Side.Left) {
            GetTree().ChangeSceneTo(Victory);
        } else {
            GetTree().ChangeSceneTo(Defeat);
        }
    }

    void SetUIDepth() {
        UIDepth = 100;
        if (OS.HasTouchscreenUiHint()) {
            UIDepth = 20;
        }
    }

    void SetLanes() {
        Vector2 displaySize = GetViewport().GetVisibleRect().Size;
        float bottom = displaySize.y * -0.1f;
        float step = (displaySize.y - UIDepth + bottom) / -9f;
        for (float i = 8; i > -1; i--) {
            Lanes.Add(bottom + (step*i));
        }
        float cursorGap = displaySize.x*0.03f;
        LCursor.Position = new Vector2(cursorGap, Lanes[0]);
        RCursor.Position = new Vector2(cursorGap*-1f, Lanes[0]);
    }
}
