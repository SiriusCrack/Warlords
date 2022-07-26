using Godot;
using Godot.Collections;
using System;

public class Battle : Node {
    // Parents
    Main Main;

    // UI Setup
    [Export] PackedScene DefaultUI;
    [Export] PackedScene TouchUI;
    public Array<float> Lanes = new Array<float>();
    float LaneStep;
    const int LaneCount = 9;
    UI UI;

    // Camp Setup
    public enum Side {Left, Right};
    PackedScene LeftCampScene;
    PackedScene RightCampScene;
    Array<PackedScene> LeftCampUnits;
    Array<PackedScene> RightCampUnits;
    bool IsLeftPlayable;
    bool IsRightPlayable;
    PackedScene AIScene;
    Camp LeftCamp;
    Camp RightCamp;
    
    // End Scenes
    public enum EndState {Victory, Defeat};
    [Export] PackedScene Victory;
    [Export] PackedScene Defeat;

    // Children
    Node Battlefield;

    public void SetUp (
        Main main,
        PackedScene leftCampScene, 
        PackedScene rightCampScene, 
        Array<PackedScene> leftCampUnits, 
        Array<PackedScene> rightCampUnits, 
        bool isLeftPlayable, 
        bool isRightPlayable, 
        PackedScene aiScene
    ) {
        Main = main;
        LeftCampScene = leftCampScene;
        RightCampScene = rightCampScene;
        LeftCampUnits = leftCampUnits;
        RightCampUnits = rightCampUnits;
        IsLeftPlayable = isLeftPlayable;
        IsRightPlayable = isRightPlayable;
        AIScene = aiScene;
    }

    public override void _Ready() {
        MakeUI();
        SetLanes();
        MakeCamp(Side.Left);
        MakeCamp(Side.Right);
        MakeBattlefield();
        MoveChild(UI, 3);
    }

    public float GetLaneStep() {
        return LaneStep;
    }

    public void AddToBattlefield(Unit unit) {
        Battlefield.AddChild(unit);
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
        Main.GameOver(endState, endScene, GetNode("."));
    }

    void MakeUI() {
        PackedScene ui;
        if (OS.HasTouchscreenUiHint()) {
            ui = TouchUI;
        } else {
            ui = DefaultUI;
        }
        UI = ui.Instance<UI>();
        UI.SetUp(this);
        AddChild(UI);
    }
    
    void MakeCamp(Side side) {
        Camp camp;
        Array<PackedScene> campUnits;
        bool isCampPlayable;
        switch (side) {
            case Side.Left: {
                camp = LeftCampScene.Instance<Camp>();
                campUnits = LeftCampUnits;
                isCampPlayable = IsLeftPlayable;
                break;
            }
            default: {
                camp = RightCampScene.Instance<Camp>();
                campUnits = RightCampUnits;
                isCampPlayable = IsRightPlayable;
                break;
            }
        }
        camp.SetUp (
            this,
            UI,
            campUnits,
            side,
            isCampPlayable
        );
        AddChild(camp);
        if (!isCampPlayable) GiveAI(camp, campUnits);
    }

    void GiveAI(Camp camp, Array<PackedScene> campUnits) {
        AI ai = AIScene.Instance<AI>();
        ai.SetUp (
            camp,
            campUnits.Count,
            LaneCount
        );
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
    }

    void MakeBattlefield() {
        Battlefield = new Node();
        Battlefield.Name = "Battlefield";
        AddChild(Battlefield);
    }
}
