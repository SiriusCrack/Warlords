using Godot;
using Godot.Collections;
using System;

public class Camp : Control {
    // Parents
    Battle Battle;
    UI UI;

    // Spawning
    Array<PackedScene> UnitScenes;
    Battle.Side Side;
    bool IsPlayable;
    const float SpawnPoint = 100;

    // Children
    SpawnTimerContainer SpawnTimerContainer;
    Cursor Cursor;
    Area2D Goal;

    public void SetUp (
        Battle battle,
        UI ui,
        Array<PackedScene> unitScenes,
        Battle.Side side,
        bool isPlayable
    ) {
        Battle = battle;
        UI = ui;
        UnitScenes = unitScenes;
        Side = side;
        IsPlayable = isPlayable;
    }

    public override void _Ready() {
        Cursor = GetNode<Cursor>("Cursor");
        Goal = GetNode<Area2D>("Goal");
        if (Side == Battle.Side.Right) AlignRight();
        SetCursor();
        SetSpawnTimerContainer();
        SetGoal();
    }

    public override void _Process(float delta) {
        if (IsPlayable) {
            CheckInput();
        }
    }

    public SpawnTimerContainer GetSpawnTimerContainer() {
        return SpawnTimerContainer;
    }

    public Cursor GetCursor() {
        return Cursor;
    }

    public void SpawnUnit(int unitAddress) {
        if (SpawnTimerContainer.CheckTimer(unitAddress)) {
            Node unitScene = UnitScenes[unitAddress].Instance<Node>();
            Unit unit = unitScene.GetNode<Unit>("Unit");
            unitScene.RemoveChild(unit);
            unitScene.QueueFree();
            SetUnit(unit);
            Battle.AddToBattlefield(unit);
            SpawnTimerContainer.ResetTimer(unitAddress);
        }
    }

    public void OnGoalEntered(Unit unit) {
        UI.OnGoalEntered(unit, Side);
    }

    void AlignRight() {
        Control control = GetNode<Control>(".");
        control.AnchorLeft = 1;
        control.AnchorRight = 1;
        control.RectScale = new Vector2(-1, 1);
    }

    void SetCursor() {
        Cursor.SetUp (
            Battle,
            this,
            IsPlayable
        );
        
    }

    void SetUnit(Unit unit) {
        unit.SetUp (
            this,
            Cursor,
            Side,
            SpawnPoint
        );
        
    }

    void SetSpawnTimerContainer() {
        SpawnTimerContainer = UI.GetSpawnTimerContainer(Side, IsPlayable);
        SpawnTimerContainer.SetUp (
            this, 
            IsPlayable,
            UnitScenes 
        );
    }

    void SetGoal() {
        int fromBit;
        int toBit;
        switch (Side) {
			case Battle.Side.Left:
                fromBit = 16;
                toBit = 32;
				break;
			default:
                fromBit = 0;
                toBit = 16;
				break;
		}
        for (int i = fromBit; i < toBit; i++) {
            Goal.SetCollisionMaskBit(i, true);
        }
    }

    void CheckInput() {
        if (Input.IsActionJustPressed("spawn")) {
            SpawnUnit(SpawnTimerContainer.GetUnitSelect());
        }
    }
}
