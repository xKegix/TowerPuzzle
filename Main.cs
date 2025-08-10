using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Game;

public partial class Main : Node
{
	private Sprite2D cursor;
	private PackedScene buildingScene;
	private Button placeBuildingButton;
	private TileMapLayer highlightTilemapLayer;

	// tracking hovered cell.
	private Vector2? hoveredGridCell;
	// create notepad to remember taken grid positions in game.
		// > could use 2D array, but more memory???
	// stores co-ordinate so vector2.
	// create like new array when game starts. Maybe initialise it already?
	private HashSet<Vector2> occupiedCells = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		buildingScene = GD.Load<PackedScene>("res://scenes/building/Building.tscn");
		cursor = GetNode<Sprite2D>("Cursor");
		placeBuildingButton = GetNode<Button>("PlaceBuildingButton");
		highlightTilemapLayer = GetNode<TileMapLayer>("HighlightTileMapLayer");
							//GD.Print($"TileMap Layer Found: {highlightTileMapLayer != null}");

		
		highlightTilemapLayer.Modulate = new Color(1, 1, 1, 0.5f); // 50% transparent

		cursor.Visible = false;

		placeBuildingButton.Pressed += OnButtonPressed;
	}

	// when action provoked.
	// call place building function.
	public override void _UnhandledInput(InputEvent evt)
	{
		if (cursor.Visible && evt.IsActionPressed("left_click") && !occupiedCells.Contains(GetMouseGridCellPosition()))
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

		// if currently placing building and hovered cell has no value or value set for hovered cell not equal to grid position.
		if (cursor.Visible && (!hoveredGridCell.HasValue || hoveredGridCell.Value != gridPosition))
		{
			hoveredGridCell = gridPosition;
			UpdateHighlightTileMapLayer();
		}
	}

	private Vector2 GetMouseGridCellPosition()
	{
		var mousePosition = highlightTilemapLayer.GetGlobalMousePosition(); // get mouse position.
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
		occupiedCells.Add(gridPosition);

		hoveredGridCell = null;
		UpdateHighlightTileMapLayer();
	}

	// implement safety check.
	/* 
	> if mouse NOT hovering over grid cell - not in build mode so stop,
	> if yes continue.
	> start loop that runs X. 
	> hoverCell range 5 tiles wide area (-3 / +3).
	> loop for Y.
	
	> clear tile at spot.
	*/
	private void UpdateHighlightTileMapLayer()
	{
		
		//GD.Print("UpdateHighlight CALLED!");
		highlightTilemapLayer.Clear();

		if (!hoveredGridCell.HasValue)
		{
			//GD.PrintErr("ERROR: TileMap layer is null!");
			return;
		}
		 //GD.Print("TileMap layer exists");
		// GD.Print($"Mouse grid position: {hoveredGridCell}");
		for (var x = hoveredGridCell.Value.X - 3; x <= hoveredGridCell.Value.X + 3; x++) // start from left, end right, move right one tile each step.
		{
			for (var y = hoveredGridCell.Value.Y - 3; y <= hoveredGridCell.Value.Y + 3; y++)
			{
				highlightTilemapLayer.SetCell(new Vector2I((int)x, (int)y), 0, Vector2I.Zero);
			}
		}
	} 


	private void OnButtonPressed()
	{
		cursor.Visible = true;
	}
}
