using Godot;
using System;

public class Menu : Control {
    [Export] PackedScene Game;
    void OnStartButtonPressed() {
        GetParent<Main>().StartGame();
    }
}