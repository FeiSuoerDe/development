using Godot;
using System;

public partial class World : Node
{
	[Export]
	public TileMapLayer Terr;
	[Export]
	public TileMapLayer River;
	[Export]
	public TileMapLayer Ecolog;

	public override void _Ready()
	{
		MakeMap();
	}


	public override void _Process(double delta)
	{
	}
	private void MakeMap()
	{
		setTerr();
	}
	private void setTerr()
	{

	}
}
