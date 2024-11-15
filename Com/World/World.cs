using Godot;
using System;

/// <summary>
/// 世界类，包含创造地图和初始化噪音等相关功能，用于在游戏中构建地图场景
/// </summary>
public partial class World : Node
{
	// 地形层，可在Godot编辑器中赋值，用于绘制地形相关的图块，例如陆地、水域等不同地形类型
	[Export]
	public TileMapLayer TerrainLayer;

	// 河流层，可在Godot编辑器中赋值，用于绘制河流相关元素，比如河道等
	[Export]
	public TileMapLayer RiverLayer;

	// 生态层，可在Godot编辑器中赋值，用于呈现生态相关的内容，像植被分布等体现生态特点的元素
	[Export]
	public TileMapLayer EcologyLayer;

	// 噪声生成器，用于生成伪随机的噪声数据，以此来确定不同位置的地形特征等，可在编辑器配置其相关属性
	[Export]
	public FastNoiseLite NoiseGenerator;

	// 随机种子，用于初始化噪声生成器，不同的种子会生成不同的随机地形效果，可在编辑器中指定
	[Export]
	public int Seed;

	// 地图的尺寸，默认为宽和高都是500，可在编辑器中根据实际需求调整地图大小
	[Export]
	public Vector2I MapDimensions = new Vector2I(500, 500);

	// 地形参数相关，用于影响地形生成时不同地形类型的高度、起伏等相关特征，可在编辑器中调整范围
	[Export(PropertyHint.Range, "-5,5,1"), ExportGroup("地形参数")]
	public float Elevation;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float Moisture;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float Temperature;

	// 地形阈值数组，用于界定不同地形类型在噪声值范围内的划分界限
	// 例如，当噪声值低于DeepSeaThreshold时，对应区域判定为深海地形
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

	// 用于存储每个地图格子对应的地形数据，二维数组的维度对应地图的宽和高
	public TileData[,] tileDatas;

	// 地形索引数组，每个元素对应一种地形类型的索引坐标，用于在TileMap中设置对应的图块
	// 例如，new Vector2I(1, 0) 对应DeepSea地形在TileMap中的图块索引坐标
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

	// 在节点准备就绪时调用，用于初始化噪声和地图相关数据
	public override void _Ready()
	{
		InitializeNoise();
		InitializeMap();
	}

	// 每帧物理处理时调用，检测是否按下了"enter"键，如果按下则重新生成地图
	public override void _PhysicsProcess(double delta)
	{
		if (Input.IsActionJustPressed("enter"))
		{
			GenerateMap();
		}
	}

	// 初始化地形数据数组，根据地图尺寸创建对应的二维数组来存储每个格子的地形数据
	public void InitializeMap()
	{
		tileDatas = new TileData[MapDimensions.X, MapDimensions.Y];
	}

	// 初始化噪声生成器，通过给定的种子来设置，确保每次生成的噪声数据具有可重复性（基于相同种子）
	// 如果为0则生成种子
	public void InitializeNoise()
	{
		if (Seed == 0)
		{
			Random random = new Random();
			int randomNumber = random.Next();
			Seed = randomNumber;
		}
		NoiseGenerator.SetSeed(Seed);
	}

	// 对外公开的生成地图的方法，内部调用设置地形的方法来构建整个地图的地形布局
	private void GenerateMap()
	{
		SetTerrain();
	}

	// 设置整个地图的地形，先清空已有的地形层数据，然后遍历地图的每个格子，根据噪声值来设置对应格子的地形图块
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

	// 根据给定的坐标和噪声值，设置对应地图格子的地形图块
	// 通过获取该噪声值对应的地形索引，然后在地形层中设置对应位置的图块
	private void SetTerrainTile(int x, int y, double noiseValue)
	{
		Vector2I position = new Vector2I(x, y);
		int index = GetTerrainIndex(noiseValue);
		TerrainLayer.SetCell(position, 0, TerrainIndices[index]);
	}

	// 根据传入的噪声值，确定其对应的地形索引
	// 遍历地形阈值数组，当噪声值小于某个阈值时，返回对应的索引，若遍历完都不满足则返回最后一个索引
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
		return elevationThresholds.Length; // 返回最后一个索引，表示最后一种地形类型（如果噪声值超过了所有阈值）
	}
}