using Godot;
using Godot.Collections;
using System;

public class Battle : Node {
    // UI Setup
    [Export] PackedScene DefaultUI;
    [Export] PackedScene TouchUI;
    public Array<float> Lanes = new Array<float>();
    public float LaneStep;
    const int LaneCount = 9;
    UI UI;

    // Camp Setup
    public enum Side {Left, Right};
    public PackedScene LeftCampScene;
    public PackedScene RightCampScene;
    public Array<PackedScene> LeftCampUnits;
    public Array<PackedScene> RightCampUnits;
    public bool IsLeftPlayable;
    public bool IsRightPlayable;
    public PackedScene AI;
    Camp LeftCamp;
    Camp RightCamp;
    
    // End Scenes
    public enum EndState {Victory, Defeat};
    [Export] PackedScene Victory;
    [Export] PackedScene Defeat;

    public Node Battlefield;

    public override void _Ready() {
        MakeUI();
        MakeCamp(Side.Left);
        MakeCamp(Side.Right);
        MakeBattlefield();
        MoveChild(UI, 3);
        SetLanes();
    }

    public void GameOver(Side winner) {
        EndState endState;
        PackedScene endScene;
        if (
            winner == Side.Left && IsLeftPlayable ||
            winner == Side.Right && IsRightPlayable
        ) {
            endState = EndState.Victory;
            endScene = Victory;
        } else {
            endState = EndState.Defeat;
            endScene = Defeat;
        }
        GetParent<Main>().GameOver(endState, endScene, GetNode("."));
    }

    void MakeUI() {
        PackedScene ui;
        if (OS.HasTouchscreenUiHint()) {
            ui = TouchUI;
        } else {
            ui = DefaultUI;
        }
        UI = ui.Instance<UI>();
        AddChild(UI);
    }
    
    void MakeCamp(Side side) {
        Camp camp;
        switch (side) {
            case Side.Left: {
                LeftCamp = LeftCampScene.Instance<Camp>();
                LeftCamp.UnitScenes = LeftCampUnits;
                LeftCamp.IsPlayable = IsLeftPlayable;
                if (!IsLeftPlayable) {
                    GiveAI(LeftCamp);
                }
                LeftCamp.Side = Side.Left;
                camp = LeftCamp;
                break;
            }
            default: {
                RightCamp = RightCampScene.Instance<Camp>();
                RightCamp.UnitScenes = RightCampUnits;
                RightCamp.IsPlayable = IsRightPlayable;
                if (!IsRightPlayable) {
                    GiveAI(RightCamp);
                }
                RightCamp.Side = Side.Right;
                Control control = RightCamp.GetNode<Control>(".");
                control.AnchorLeft = 1;
                control.AnchorRight = 1;
                control.RectScale = new Vector2(-1, 1);
                camp = RightCamp;
                break;
            }
        }
        camp.Battle = this;
        camp.UI = UI;
        AddChild(camp);
    }

    void GiveAI(Camp camp) {
        AI ai = AI.Instance<AI>();
        ai.Camp = camp;
        ai.UnitRange = RightCampUnits.Count;
        ai.LaneRange = LaneCount;
        camp.AddChild(ai);
    }

    void SetLanes() {
        Vector2 displaySize = GetViewport().GetVisibleRect().Size;
        float verticalPadding = displaySize.y * 0.05f;
        float uiDepth = UI.GetUIDepth() + verticalPadding;
        float bottom = -verticalPadding;
        LaneStep = (displaySize.y - uiDepth + bottom) / LaneCount;
        for (float i = LaneCount-1; i > -1; i--) {
            Lanes.Add(bottom - (LaneStep*i));
        }
        LeftCamp.SetCursor();
        RightCamp.SetCursor();
    }

    void MakeBattlefield() {
        Battlefield = new Node();
        Battlefield.Name = "Battlefield";
        AddChild(Battlefield);
    }
}
