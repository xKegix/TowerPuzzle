using Godot;
using System;
using System.Runtime.CompilerServices;

namespace Game;

public partial class Main : Node2D
{
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
		cursor = GetNode<Sprite2D>("Cursor");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");

		cursor.Visible = false;

		placeBuildingButton.Pressed += OnButtonPressed;
	}

	// when action provoked.
	// call place building function.
	public override void _UnhandledInput(InputEvent evt)
	{
		if (cursor.Visible && evt.IsActionPressed("left_click"))
		{
			PlaceBuildingAtMousePosition();
			cursor.Visible = false;
		}
	}



	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var gridPosition = GetMouseGridCellPosition();
		cursor.GlobalPosition = gridPosition * 64; // square pos = pixel pos.

	}

	private Vector2 GetMouseGridCellPosition()
	{
		var mousePosition = GetGlobalMousePosition(); // get mouse position.
		var gridPosition = mousePosition / 64;  // divide x,y by 64 to get grid pos.
		gridPosition = gridPosition.Floor(); // round nearest whole number.
		return gridPosition;
	}

	// create instances of building.
	// add child node to parent node - Main.
	private void PlaceBuildingAtMousePosition()
	{
		var building = buildingScene.Instantiate<Node2D>();
		AddChild(building);

		var gridPosition = GetMouseGridCellPosition();
		building.GlobalPosition = gridPosition * 64;
	}

	private void OnButtonPressed()
	{
		cursor.Visible = true;
	}
}


 