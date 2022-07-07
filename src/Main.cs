using Godot;
using System;

public class Main : Node {
    [Export] PackedScene StartScene;
    [Export] PackedScene BattleScene;
    [Export] PackedScene WinScene;
    [Export] PackedScene LoseScene;
    AudioStreamPlayer BGMusicPlayer;
    [Export] AudioStream VictoryMusic;
    [Export] AudioStream DefeatMusic;

    public override void _Ready() {
        BGMusicPlayer = GetNode<AudioStreamPlayer>("BGMusicPlayer");
        AddChild(StartScene.Instance<Node>());
    }

    public void StartGame() {
        GetNode<Node>("Menu").QueueFree();
        AddChild(BattleScene.Instance<Node>());
    }

    public void Victory() {
        GetNode<Node>("Battle").QueueFree();
        AddChild(WinScene.Instance<Node>());
        BGMusicPlayer.Stream = VictoryMusic;
        BGMusicPlayer.Play();
    }

    public void Defeat() {
        GetNode<Node>("Battle").QueueFree();
        AddChild(LoseScene.Instance<Node>());
        BGMusicPlayer.Stream = DefeatMusic;
        BGMusicPlayer.Play();
    }
}
