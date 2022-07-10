using Godot;
using Godot.Collections;
using System;

public class Camp : Control {
    public Battle Battle;
    public UI UI;
    public Array<PackedScene> UnitScenes;
    public bool IsPlayable;
    public Battle.Side Side;
    public SpawnTimerContainer SpawnTimerContainer;
    public Cursor Cursor;
    Area2D Goal;

    public override void _Ready() {
        Cursor = GetNode<Cursor>("Cursor");
        Goal = GetNode<Area2D>("Goal");
        SetSpawnTimerContainer();
        SetGoal();
    }

    public override void _Process(float delta) {
        if (IsPlayable) {
            GetInput();
        }
    }

    public void SetCursor() {
        Cursor.Camp = this;
        Cursor.Position = new Vector2(Cursor.Position.x, Battle.Lanes[0]);
        Cursor.Lane = 0;
        Cursor.LaneCount = Battle.Lanes.Count;
    }

    public void SpawnUnit(int unitAddress) {
        if (SpawnTimerContainer.CheckTimer(unitAddress)) {
            Node unitScene = UnitScenes[unitAddress].Instance<Node>();
            Unit unit = unitScene.GetNode<Unit>("Unit");
            unitScene.RemoveChild(unit);
            unitScene.QueueFree();
            unit.Camp = this;
            unit.SetDirection(Side);
            Battle.Battlefield.AddChild(unit);
            unit.GlobalPosition = new Vector2(Cursor.GlobalPosition.x-50, Cursor.GlobalPosition.y);
            unit.SetCollision(Cursor.Lane, Side);
            SpawnTimerContainer.ResetTimer(unitAddress);
        }
    }

    void OnGoalEntered(Area2D area) {
        UI.OnGoalEntered(area);
    }

    void SetGoal() {
        switch (Side) {
			case Battle.Side.Left: {
				for (int i = 16; i < 32; i++) {
					Goal.SetCollisionMaskBit(i, true);
				}
				break;
			}
			default: {
				for (int i = 0; i < 16; i++) {
					Goal.SetCollisionMaskBit(i, true);
				}
				break;
			}
		}
    }

    void SetSpawnTimerContainer() {
        SpawnTimerContainer = UI.GetSpawnTimerContainer(Side, IsPlayable);
        SpawnTimerContainer.SetSpawnTimers(UnitScenes);
        SpawnTimerContainer.Camp = this;
        SpawnTimerContainer.IsPlayable = IsPlayable;
    }

    void GetInput() {
        if (Input.IsActionJustPressed("spawn")) {
            SpawnUnit(SpawnTimerContainer.UnitSelect);
        }
    }
}
