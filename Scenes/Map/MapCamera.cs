using Godot;
using System;

public class MapCamera : Camera2D {
    const float MIN_ZOOM = 1;
    const float MAX_ZOOM = 4;
    const float ZOOM_INCREMENT = 0.1f;
    const float ZOOM_RATE = 5;
    float TargetZoom;

    public override void _Ready() {
        TargetZoom = 4;
    }

    public override void _Process(float delta) {
        CheckInput();
    }

    public override void _PhysicsProcess(float delta) {
        Zoom = Vector2Lerp (
            Zoom, 
            TargetZoom * Vector2.One, 
            ZOOM_RATE * delta
        );
        SetPhysicsProcess(!Mathf.IsEqualApprox(Zoom.x, TargetZoom));
    }

    public override void _UnhandledInput(InputEvent inputEvent) {
        if (inputEvent is InputEventMouseMotion) {
            InputEventMouseMotion mouseMotionInput = (InputEventMouseMotion)inputEvent;
            if (mouseMotionInput.ButtonMask == 4) {
                Position -= mouseMotionInput.Relative * Zoom;
            }
        }
        if (inputEvent is InputEventMouseButton) {
            InputEventMouseButton mouseButtonInput = (InputEventMouseButton)inputEvent;
            if (mouseButtonInput.ButtonMask == 8) {
                ZoomIn();
            }
            if (mouseButtonInput.ButtonMask == 16) {
                ZoomOut();
            }
        }
    }
    
    float Lerp(float firstFloat, float secondFloat, float by) {
        return firstFloat * (1 - by) + secondFloat * by;
    }

    Vector2 Vector2Lerp(Vector2 firstVector, Vector2 secondVector, float by) {
        float retX = Lerp(firstVector.x, secondVector.x, by);
        float retY = Lerp(firstVector.y, secondVector.y, by);
        return new Vector2(retX, retY);
    }

    void ZoomIn() {
        TargetZoom = Math.Max(TargetZoom - ZOOM_INCREMENT, MIN_ZOOM);
        SetPhysicsProcess(true);
    }

    void ZoomOut() {
        TargetZoom = Math.Min(TargetZoom + ZOOM_INCREMENT, MAX_ZOOM);
        SetPhysicsProcess(true);
    }

    void CheckInput() {
        if (Input.IsActionPressed("right")) {
            Position += Vector2.Right*100;
        }
        if (Input.IsActionPressed("left")) {
            Position += Vector2.Left*100;
        }
        if (Input.IsActionPressed("down")) {
            Position += Vector2.Down*100;
        }
        if (Input.IsActionPressed("up")) {
            Position += Vector2.Up*100;
        }
    }
}