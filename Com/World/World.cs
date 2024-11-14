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

	[Export]
	public FastNoiseLite fastNoiseLite;

	[Export]
	public int seed;

	[Export]
	public Vector2I MapSize = new Vector2I(500, 500);

	[Export(PropertyHint.Range, "-5,5,1"), ExportGroup("地形参数")]
	public float Elevation;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float moisture;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float temperature;

	// 地形配置数组
	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold1 = -0.3f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold2 = 0.02f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold3 = 0.05f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold4 = 0.5f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold5 = 0.7f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold6 = 0.75f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float Threshold7 = 0.8f;

	// 地形索引数组
	public Vector2I[] TerrainIndices = new Vector2I[]
	{
		new Vector2I(1, 0),
		new Vector2I(2, 0),
		new Vector2I(3, 0),
		new Vector2I(4, 0),
		new Vector2I(5, 0),
		new Vector2I(6, 0),
		new Vector2I(7, 0),
		new Vector2I(8, 0),
		new Vector2I(9, 0)
	};

	public override void _Ready()
	{
		redNoise();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("enter"))
		{
			MakeMap();
		}
	}

	// 初始化噪声
	public void redNoise()
	{
		fastNoiseLite.SetSeed(seed);
	}

	private void MakeMap()
	{
		setTerr();
	}

	private void setTerr()
	{
		Terr.Clear();
		for (int x = 0; x < MapSize.X; x++)
		{
			for (int y = 0; y < MapSize.Y; y++)
			{
				double noiseValue = fastNoiseLite.GetNoise2D(x, y);
				SetTerrainTile(x, y, noiseValue);
			}
		}
	}

	private void SetTerrainTile(int x, int y, double noiseValue)
	{
		Vector2I position = new Vector2I(x, y);
		int index = GetTerrainIndex(noiseValue);
		Terr.SetCell(position, 0, TerrainIndices[index]);
	}

	private int GetTerrainIndex(double noiseValue)
	{
		float[] thresholds = new float[] { Threshold1, Threshold2, Threshold3, Threshold4, Threshold5, Threshold6, Threshold7 };
		for (int i = 0; i < thresholds.Length; i++)
		{
			if (noiseValue < thresholds[i])
			{
				return i;
			}
		}
		return thresholds.Length; // 返回最后一个索引
	}
}