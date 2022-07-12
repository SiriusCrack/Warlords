using Godot;
using Godot.Collections;
using System;

public class SpawnTimerContainer : HBoxContainer {
    // Parent
    Camp Camp;
    
    bool IsPlayable;
    int UnitSelect;

    // Children
    Array<SpawnTimerUI> SpawnTimerUIs = new Array<SpawnTimerUI>();

    public void SetUp (
        Camp camp,
        bool isPlayable,
        Array<PackedScene> unitScenes
    ) {
        Camp = camp;
        IsPlayable = isPlayable;
        UnitSelect = 0;
        int i = 0;
        foreach (PackedScene unitScene in unitScenes) {
            Node unit = unitScene.Instance<Node>();
            SpawnTimerUI spawnTimerUI = unit.GetNode<SpawnTimerUI>("SpawnTimerUI");
            unit.RemoveChild(spawnTimerUI);
            unit.QueueFree();
            SetSpawnTimerUI(spawnTimerUI, i);
            AddChild(spawnTimerUI);
            SpawnTimerUIs.Add(spawnTimerUI);
            i++;
        }
    }

    public override void _Process(float delta) {
        if (IsPlayable) {
            CheckInput();
        }
    }

    public int GetUnitSelect() {
        return UnitSelect;
    }

    public bool CheckTimer(int timerAddress) {
        return SpawnTimerUIs[timerAddress].CheckTimer();
    }

    public void ResetTimer(int timerAddress) {
        foreach (SpawnTimerUI spawnTimerUI in SpawnTimerUIs) {
            spawnTimerUI.ResetTimer();
        }
    }

    public void SpawnSelect(int unitSelect) {
        Camp.SpawnUnit(unitSelect);
    }

    public void SpawnSelectLeft() {
        SpawnTimerUIs[UnitSelect].GetNode<TextureRect>("Select").Visible = false;
        UnitSelect--;
        if (UnitSelect < 0) UnitSelect = SpawnTimerUIs.Count-1;
        if (!OS.HasTouchscreenUiHint()) {
            SpawnTimerUIs[UnitSelect].GetNode<TextureRect>("Select").Visible = true;
        }
    }

    public void SpawnSelectRight() {
        SpawnTimerUIs[UnitSelect].GetNode<TextureRect>("Select").Visible = false;
        UnitSelect++;
        UnitSelect = UnitSelect %SpawnTimerUIs.Count;
        if (!OS.HasTouchscreenUiHint()) {
            SpawnTimerUIs[UnitSelect].GetNode<TextureRect>("Select").Visible = true;
        }
    }

    void SetSpawnTimerUI(SpawnTimerUI spawnTimerUI, int timerAddress) {
        spawnTimerUI.SetUp (
            this,
            timerAddress
        );
    }

    void CheckInput() {
        if (Input.IsActionJustPressed("left")) {
            SpawnSelectLeft();
        }
        if (Input.IsActionJustPressed("right")) {
            SpawnSelectRight();
        }
    }
}
