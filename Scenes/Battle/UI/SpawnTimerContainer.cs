using Godot;
using Godot.Collections;
using System;

public class SpawnTimerContainer : HBoxContainer {
    public Camp Camp;
    public bool IsPlayable;
    public int UnitSelect = 0;
    Array<SpawnTimerUI> SpawnTimerUIs = new Array<SpawnTimerUI>();

    public override void _Process(float delta) {
        if (IsPlayable) {
            GetInput();
        }
    }

    public bool CheckTimer(int timerAddress) {
        return SpawnTimerUIs[timerAddress].CheckTimer();
    }

    public void ResetTimer(int timerAddress) {
        foreach (SpawnTimerUI spawnTimerUI in SpawnTimerUIs) {
            spawnTimerUI.ResetTimer();
        }
    }

    public void SetSpawnTimers(Array<PackedScene> unitScenes) {
        int i = 0;
        
        foreach (PackedScene unitScene in unitScenes) {
            Node unit = unitScene.Instance<Node>();
            SpawnTimerUI spawnTimerUI = unit.GetNode<SpawnTimerUI>("SpawnTimerUI");
            SpawnTimerUIs.Add(spawnTimerUI);
            unit.RemoveChild(spawnTimerUI);
            unit.QueueFree();
            spawnTimerUI.SpawnTimerContainer = this;
            spawnTimerUI.TimerAddress = i;
            AddChild(spawnTimerUI);
            i++;
        }
    }

    public void SelectSpawn(int direction) {
        SpawnTimerUIs[UnitSelect].GetNode<TextureRect>("Select").Visible = false;
        UnitSelect += direction;
        if (UnitSelect < 0) {
            UnitSelect = SpawnTimerUIs.Count-1;
        } else {
            UnitSelect = UnitSelect % SpawnTimerUIs.Count;
        }
        if (!OS.HasTouchscreenUiHint()) {
            SpawnTimerUIs[UnitSelect].GetNode<TextureRect>("Select").Visible = true;
        }
    }

    void GetInput() {
        if (Input.IsActionJustPressed("right")) {
            SelectSpawn(1);
        }
        if (Input.IsActionJustPressed("left")) {
            SelectSpawn(-1);
        }
    }
}
