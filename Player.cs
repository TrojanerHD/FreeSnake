using Godot;
using System;

public class Player : Godot.Line2D {
    [Export] public int speed = 140;
    [Export] public float timeout = 0.02f;
    [Export] public bool invincible = false;
    private float time = 0;
    private bool paused = false;
    public int pendingTiles = 0;
    private float timePendingTiles = 0;
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        for (int i = 0; i < 50; i++)
            AddPoint(Points[0], 0);
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        if (Input.IsActionJustPressed("ui_cancel")) {
            paused = !paused;
            if (paused) Hide();
            else Show();
        }
        if (paused) return;
        if (!invincible && FindIntersections()) {
            ClearPoints();
            AddPoint(new Vector2(0, 0));
            ((Apple)GetNode("/root/Game/Apple")).RandomizePosition();
            _Ready();
        }
        if (GetPointCount() == 0) return;
        time += delta;
        timePendingTiles += delta;
        if (time > timeout) {
            for (int i = 0; i < GetPointCount() - 1; i++)
                SetPointPosition(i, GetPointPosition(i + 1));
            time = 0;
        }
        if (timePendingTiles > 0.05 && pendingTiles > 0) {
            pendingTiles--;
            AddPoint(Points[0], 0);
            if (GetPointCount() > 1000) {
                for (int i = 0; i < GetPointCount(); i += 2) RemovePoint(i);
                timeout *= 2;
            }
            timePendingTiles = 0;
        }
        Vector2 mousePos = GetGlobalMousePosition();
        Vector2 velocity = Points[GetPointCount() - 1].DirectionTo(mousePos);

        SetPointPosition(GetPointCount() - 1, GetPointPosition(GetPointCount() - 1) + velocity * speed * delta);
    }

    private bool FindIntersections() {
        for (int i = 1; i < GetPointCount(); i++) {
            if (Math.Abs(GetPointCount() - 1 - i) < 2) continue;
            Vector2 begin0 = Points[i - 1];
            Vector2 end0 = Points[i];

            Vector2 begin1 = Points[GetPointCount() - 2];
            Vector2 end1 = Points[GetPointCount() - 1];

            if (GetSegmentIntersection(begin0, end0, begin1, end1)) return true;
        }
        return false;
    }

    private bool GetSegmentIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
        Vector2 cd = d - c;
        Vector2 ab = b - a;

        float div = cd.y * ab.x - cd.x * ab.y;

        if (Math.Abs(div) > 0.001) {
            float ua = (cd.x * (a.y - c.y) - cd.y * (a.x - c.x)) / div;
            float ub = (ab.x * (a.y - c.y) - ab.y * (a.x - c.x)) / div;
            if (ua >= 0.0f && ua <= 1.0f && ub >= 0.0f && ub <= 1.0f) return true;
        }

        return false;
    }
}