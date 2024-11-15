using Godot;
using System;


/// <summary>
/// 世界类，包含创造地图和初始化噪音
/// </summary>
public partial class World : Node
{
	[Export]
	public TileMapLayer TerrainLayer;

	[Export]
	public TileMapLayer RiverLayer;

	[Export]
	public TileMapLayer EcologyLayer;

	[Export]
	public FastNoiseLite NoiseGenerator;

	[Export]
	public int Seed;

	[Export]
	public Vector2I MapDimensions = new Vector2I(500, 500);

	[Export(PropertyHint.Range, "-5,5,1"), ExportGroup("地形参数")]
	public float Elevation;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float Moisture;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float Temperature;



	// 地形阈值数组
	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float DeepSeaThreshold = -0.3f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float SeaThreshold = 0.02f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float ShallowWaterThreshold = 0.05f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float GrasslandThreshold = 0.3f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float HighlandThreshold = 0.5f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float RockThreshold = 0.7f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float MountainThreshold = 0.8f;

	[Export(PropertyHint.Range, "-1,1,0.01")]
	public float SnowMountainThreshold = 0.85f;


	public TileData[,] tileDatas;
	// 地形索引数组
	public Vector2I[] TerrainIndices = new Vector2I[]
	{
		new Vector2I(1, 0), // DeepSea
        new Vector2I(2, 0), // Sea
        new Vector2I(3, 0), // ShallowWater
        new Vector2I(5, 0), // Grassland
        new Vector2I(6, 0), // Highland
        new Vector2I(7, 0), // Rock
        new Vector2I(8, 0), // Mountain
        new Vector2I(9, 0)  // SnowMountain
    };

	public override void _Ready()
	{
		InitializeNoise();
		InitializeMap();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("enter"))
		{
			GenerateMap();
		}
	}
	// 初始化地形数据数组
	public void InitializeMap()
	{
		tileDatas = new TileData[MapDimensions.X, MapDimensions.Y];
	}
	// 初始化噪声
	public void InitializeNoise()
	{
		NoiseGenerator.SetSeed(Seed);
	}

	private void GenerateMap()
	{
		SetTerrain();
	}

	private void SetTerrain()
	{
		TerrainLayer.Clear();
		for (int x = 0; x < MapDimensions.X; x++)
		{
			for (int y = 0; y < MapDimensions.Y; y++)
			{
				double noiseValue = NoiseGenerator.GetNoise2D(x, y);
				SetTerrainTile(x, y, noiseValue);
			}
		}
	}

	private void SetTerrainTile(int x, int y, double noiseValue)
	{
		Vector2I position = new Vector2I(x, y);
		int index = GetTerrainIndex(noiseValue);
		TerrainLayer.SetCell(position, 0, TerrainIndices[index]);
	}

	private int GetTerrainIndex(double noiseValue)
	{
		float[] elevationThresholds = new float[]
		{
			DeepSeaThreshold,
			SeaThreshold,
			ShallowWaterThreshold,
			GrasslandThreshold,
			HighlandThreshold,
			RockThreshold,
			MountainThreshold,
			SnowMountainThreshold
		};
		for (int i = 0; i < elevationThresholds.Length; i++)
		{
			if (noiseValue < elevationThresholds[i])
			{
				return i;
			}
		}
		return elevationThresholds.Length; // 返回最后一个索引
	}
}