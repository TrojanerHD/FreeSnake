using Godot;
using System;

public class Apple : StaticBody2D {
    RandomNumberGenerator rng = new RandomNumberGenerator();
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready() {
        rng.Randomize();
        RandomizePosition();
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta) {
        Player player = (Player)GetNode("/root/Game/Player");
        if (player.Points[player.GetPointCount() - 1].DistanceTo(Position) < Scale.x * 2) {
            player.pendingTiles += 50;
            RandomizePosition();
        }
    }

    public void RandomizePosition() {
        int x = rng.RandiRange(0, 1024);
        int y = rng.RandiRange(0, 600);
        foreach (Vector2 point in ((Player)GetNode("/root/Game/Player")).Points)
            if (point.x == x && point.y == y) {
                RandomizePosition();
                return;
            }
        Position = new Vector2(x, y);
    }
}
