using Godot;
using System;

public class Main : Node {
    [Export] PackedScene StartScene;
    [Export] PackedScene BattleScene;
    [Export] PackedScene WinScene;
    [Export] PackedScene LoseScene;

    public override void _Ready() {
        AddChild(StartScene.Instance<Node>());
    }

    public void StartGame() {
        GetNode<Node>("Menu").QueueFree();
        AddChild(BattleScene.Instance<Node>());
    }

    public void Victory() {
        GetNode<Node>("Battle").QueueFree();
        AddChild(WinScene.Instance<Node>());
    }

    public void Defeat() {
        GetNode<Node>("Battle").QueueFree();
        AddChild(LoseScene.Instance<Node>());
    }
}
