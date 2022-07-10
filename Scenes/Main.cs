using Godot;
using System;

public class Main : Node {
    [Export] PackedScene StartScene;
    [Export] PackedScene BattleScene;
    AudioStreamPlayer BGMusicPlayer;
    [Export] AudioStream VictoryMusic;
    [Export] AudioStream DefeatMusic;

    public override void _Ready() {
        BGMusicPlayer = GetNode<AudioStreamPlayer>("BGMusicPlayer");
        AddChild(StartScene.Instance<Node>());
    }

    public void StartGame (
        PackedScene leftCamp, 
        PackedScene rightCamp, 
        Godot.Collections.Array<PackedScene> leftCampUnits, 
        Godot.Collections.Array<PackedScene> rightCampUnits,
        bool isLeftPlayable, 
        bool isRightPlayable,
        PackedScene AI
    ) {
        GetNode<Node>("Menu").QueueFree();
        Battle battleScene = BattleScene.Instance<Battle>();
        battleScene.LeftCampScene = leftCamp;
        battleScene.RightCampScene = rightCamp;
        battleScene.LeftCampUnits = leftCampUnits;
        battleScene.RightCampUnits = rightCampUnits;
        battleScene.IsLeftPlayable = isLeftPlayable;
        battleScene.IsRightPlayable = isRightPlayable;
        battleScene.AI = AI;
        AddChild(battleScene);
    }

    public void GameOver(Battle.EndState endState, PackedScene endScene, Node finishedScene) {
        finishedScene.QueueFree();
        if (endState == Battle.EndState.Victory) {
            BGMusicPlayer.Stream = VictoryMusic;
        } else {
            BGMusicPlayer.Stream = DefeatMusic;
        }
        AddChild(endScene.Instance<Node>());
        BGMusicPlayer.Play();
    }
}
