using Godot;
using System;
using System.ComponentModel;

[Tool]
public partial class World : Node
{
	[Export]
	public TileMapLayer Terr;
	[Export]
	public TileMapLayer River;
	[Export]
	public TileMapLayer Ecolog;
	[Export]
	public FastNoiseLite fastNoiseLite;
	[Export]
	public int seed;
	[Export]
	public Vector2I MapSize;
	[Export, Category("Player")]
	public double TerrDublue;

	public override void _Ready()
	{
		redNoise();

		MakeMap();
	}
	// 初始化噪声
	public void redNoise()
	{
		fastNoiseLite = new FastNoiseLite();

		fastNoiseLite.SetSeed(seed);

		if (MapSize.X == 0)
		{
			MapSize = new Vector2I(500, 500);
		}

	}
	private void MakeMap()
	{
		setTerr();
	}
	private void setTerr()
	{
		for (int X = 0; X < MapSize.X; X++)
		{
			for (int Y = 0; Y < MapSize.Y; Y++)
			{
				double noise = fastNoiseLite.GetNoise2D(X, Y);

			}

		}

	}
}
