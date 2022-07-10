using Godot;
using Godot.Collections;
using System;

public class Menu : Control {
    [Export] PackedScene LeftCamp;
    [Export] PackedScene RightCamp;
    [Export] Array<PackedScene> LeftCampUnits;
    [Export] Array<PackedScene> RightCampUnits;
    [Export] bool IsLeftPlayable;
    [Export] bool IsRightPlayable;
    [Export] PackedScene AI;

    void OnStartButtonPressed() {
        GetParent<Main>().StartGame (
            LeftCamp, 
            RightCamp, 
            LeftCampUnits, 
            RightCampUnits, 
            IsLeftPlayable, 
            IsRightPlayable,
            AI
        );
    }
}