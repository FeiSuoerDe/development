using Godot;
using System;
using System.IO;

// 世界类，负责生成地图和处理地形相关逻辑
public partial class World : Node
{
	// 地形层，用于绘制地图
	[Export]
	public TileMapLayer TerrainLayer;

	// 噪声生成器，用于生成地形噪声
	[Export]
	public FastNoiseLite NoiseGenerator;

	// 噪声种子值
	[Export]
	public int Seed;
	// 是否使用随机种子
	[Export]
	public Boolean isRandomSeed = false;

	// 地图尺寸，默认为500x500
	[Export]
	public Vector2I MapDimensions = new Vector2I(500, 500);

	// 地形参数
	[Export(PropertyHint.Range, "-5,5,1"), ExportGroup("地形参数")]
	public float Elevation;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float Moisture;

	[Export(PropertyHint.Range, "-5,5,1")]
	public float Temperature;

	// 地形阈值，用于区分不同地形类型
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

	// 存储每个地图格子的地形数据
	public TilesData[,] tilesDatas;

	// 地形索引数组，对应不同的地形类型
	public Vector2I[] TerrainIndices = new Vector2I[]
	{
		new Vector2I(1, 0), // 深海
        new Vector2I(2, 0), // 海洋
        new Vector2I(3, 0), // 浅水
        new Vector2I(5, 0), // 草地
        new Vector2I(6, 0), // 高地
        new Vector2I(7, 0), // 岩石
        new Vector2I(8, 0), // 山脉
        new Vector2I(9, 0)  // 雪山
    };

	// 地块数据数组
	public TileData[] TileData = new TileData[]
	{
		new TileData(), // 深海
        new TileData(), // 海洋
        new TileData(), // 浅水
        new TileData(), // 草地
        new TileData(), // 高地
        new TileData(), // 岩石
        new TileData(), // 山脉
        new TileData()  // 雪山
    };

	// 节点准备就绪时调用
	public override void _Ready()
	{
		InitializeMap();   // 初始化地图数据
		SetTerrain();      // 设置地形
	}

	// 未处理的输入事件
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouse)
		{
			if (eventMouse.Pressed && eventMouse.ButtonIndex == MouseButton.Left)
			{
				isLeftclick(); // 处理左键点击事件
			}
		}
	}

	// 处理左键点击事件
	public void isLeftclick()
	{
		if (Input.IsActionJustPressed("LeftClick"))
		{
			try
			{
				Vector2 pos = TerrainLayer.GetGlobalMousePosition();
				Vector2I map = TerrainLayer.LocalToMap(pos);
				var parent = GetParent();
				var childNode = parent.GetNode<WorldUi>("WorldUI");
				GD.Print(childNode);
				childNode.Stard.Disabled = false;
				childNode.tileData = tilesDatas[map.X, map.Y];
				// 获取点击位置的地形数据
				childNode.setLabel();
				childNode.setSprite(new Vector2((int)pos.X + 0.5f, (int)pos.Y + 0.5f));
				var camera = parent.GetNode<Camera>("Camera");
				camera.camera2D.Position = pos; // 将相机移动到点击位置
			}
			catch (System.Exception)
			{
				return;
			}
		}
	}

	// 初始化地图数据数组
	public void InitializeMap()
	{
		tilesDatas = new TilesData[MapDimensions.X, MapDimensions.Y];
	}

	// 初始化噪声生成器
	public void InitializeNoise()
	{
		if (isRandomSeed)
		{
			Random random = new Random();
			int randomNumber = random.Next();

			Seed = randomNumber;
		}
		NoiseGenerator.SetSeed(Seed);
	}

	// 单个地形数据实例
	TilesData tilesData = new TilesData();

	// 设置地形
	public void SetTerrain()
	{

		InitializeNoise(); // 初始化噪声生成器
		LoadTileNames();    // 加载地块名称列表

		TerrainLayer.Clear(); // 清空地形层

		for (int x = 0; x < MapDimensions.X; x++)
		{
			for (int y = 0; y < MapDimensions.Y; y++)
			{
				tilesData = new TilesData();
				tilesDatas[x, y] = tilesData;
				double noiseValue = NoiseGenerator.GetNoise2D(x, y); // 获取噪声值

				if (noiseValue <= 0)
				{
					tilesData.Elevation = 0;
				}
				else
				{
					tilesData.Elevation = (int)(noiseValue * 100);
				}

				tilesData.Name = SetTileDataName(); // 随机设置地块名称
				tilesData.pos = new Vector2I(x, y);
				SetTerrainTile(x, y, noiseValue);   // 设置对应的地形图块
				tilesData.AssignValuesBasedOnLogic(); // 根据逻辑赋值
			}
		}
	}

	// 地块名称数组
	private string[] tileNames;
	private Random random;
	private string relativePath;

	// 加载地块名称列表
	private void LoadTileNames()
	{
		random = new Random();
		relativePath = "Data/Text/TileName.txt";
		string filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);
		string fileContent = File.ReadAllText(filePath);
		tileNames = fileContent.Split(','); // 读取并分割名称
	}

	// 随机设置地块名称
	private string SetTileDataName()
	{
		int index = random.Next(tileNames.Length);
		return tileNames[index];
	}

	// 设置地形图块
	private void SetTerrainTile(int x, int y, double noiseValue)
	{
		Vector2I position = new Vector2I(x, y);
		int index = GetTerrainIndex(noiseValue); // 根据噪声值获取地形索引
		tilesData.setTileClass(index);           // 设置地形类型
		TerrainLayer.SetCell(position, 0, TerrainIndices[index]); // 设置地图块
	}

	// 根据噪声值获取对应的地形索引
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
		return elevationThresholds.Length; // 返回最后一种地形的索引
	}
}