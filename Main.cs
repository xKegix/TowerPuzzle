using Godot;
using System;

namespace Game;
public partial class Main : Node2D
{
	private Sprite2D sprite;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sprite = GetNode<Sprite2D>("Cursor");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var mousePosition = GetGlobalMousePosition(); // get mouse position.
		var gridPosition = mousePosition / 64;  // divide x,y by 64 to get grid pos.
		gridPosition = gridPosition.Floor(); // round nearest whole number.
		sprite.GlobalPosition = gridPosition * 64; // square pos = pixel pos.

	}
}
 