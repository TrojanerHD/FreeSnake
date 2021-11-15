using Godot;
using System;

public class Apple : StaticBody2D
{
	RandomNumberGenerator rng = new RandomNumberGenerator();
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rng.Randomize();
		RandomizePosition();
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Line2D player = (Line2D)GetNode("/root/Game/Player");
		if (player.Points[player.GetPointCount() - 1].DistanceTo(Position) < Scale.x * 2)
		{
			for (int i = 0; i < 50; i++)
				player.AddPoint(player.Points[0], 0);
			if (player.GetPointCount() > 1000)
			{
				for (int i = 0; i < player.GetPointCount(); i += 2) player.RemovePoint(i);
				player.timeout *= 2;
			}
			RandomizePosition();
		}
	}

	public void RandomizePosition()
	{
		int x = rng.RandiRange(0, 1024);
		int y = rng.RandiRange(0, 600);
		foreach (Vector2 point in ((Line2D)GetNode("/root/Game/Player")).Points)
			if (point.x == x && point.y == y)
			{
				RandomizePosition();
				return;
			}
		Position = new Vector2(x, y);
	}
}
