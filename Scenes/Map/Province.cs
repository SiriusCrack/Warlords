using Godot;
using System;

public class Province : Sprite {
    public override void _UnhandledInput(InputEvent inputEvent) {
        if (inputEvent is InputEventMouseButton) {
            InputEventMouseButton mouseButtonEvent = (InputEventMouseButton)inputEvent;
            if (mouseButtonEvent.ButtonMask == 1) {
                if (GetRect().HasPoint(GetLocalMousePosition())) {
                    Modulate = Color.FromHsv(GD.Randf(), GD.Randf(), GD.Randf());
                }
            }
        }
    }
}
