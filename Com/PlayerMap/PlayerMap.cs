using System;
using System.Security.Cryptography.X509Certificates;
using Godot;


public partial class PlayerMap : Node
{

	[Export]
	public Node2D node2D; // 地图节点，包含所有的 TileMapLayer
						  // 游玩地图
	[Export]
	public FastNoiseLite noise { get; set; } // 用于生成噪声数据的对象
											 // 地块数据,包含名称、海拔、湿度、年降雨量、
											 // 野生动物数、地块类型（ "深海",
											 // "海洋",
											 // "浅滩",
											 // "平原",
											 // "高地",
											 // "裸石平原",
											 // "高山",
											 // "顶峰雪原"）、
											 // 地下水含量、植被密度
	public TilesData tilesData { get; set; } // 地块数据，包含地形和环境信息

	// public Vector2I MapSize = new Vector2I(784, 784);
	public Vector2I MapSize = new Vector2I(100, 100); // 地图尺寸（宽度，高度）
	[Export]
	public int ZNumber = 20; // 地图的层数（深度）
							 // 当前层级
	private int currentLayer; // 当前显示的层级



	public override void _Process(double delta)
	{
		// 监听用户输入，PgUp和PgDn用于切换显示的层级
		if (Input.IsActionJustPressed("PgUp"))
		{
			if (currentLayer < ZNumber - 1)
			{
				currentLayer += 1; // 向上移动一层
				ShowLayer(currentLayer); // 显示新层级
			}
		}
		if (Input.IsActionJustPressed("PgDn"))
		{
			if (currentLayer > 0)
			{
				currentLayer -= 1; // 向下移动一层
				ShowLayer(currentLayer); // 显示新层级
			}
		}
	}
	public override void _Ready()
	{


		MakeMap();        // 生成地图数据和层级
		setCamera();      // 设置相机位置和缩放
		HideLayer();      // 隐藏所有层级
		currentLayer = ZNumber - 1;
		ShowLayer(currentLayer); // 显示当前层级

	}
	// layer隐藏
	// 隐藏所有层级
	public void HideLayer()
	{

		for (int i = 0; i < ZNumber; i++)
		{
			node2D.GetChild<TileMapLayer>(i).Visible = false; // 设置层级不可见
		}
	}
	// layre显示
	// 显示指定的层级
	public void ShowLayer(int layer)
	{
		if (layer >= 0 && layer < ZNumber)
		{
			// 隐藏所有层级
			HideLayer(); // 隐藏所有层级
						 // 显示当前层级
			node2D.GetChild<TileMapLayer>(layer).Visible = true; // 显示当前层级
		}
	}
	// 设置相机位置和缩放
	public void setCamera()
	{
		Node2D parent = GetParent() as Node2D;                // 获取父节点
		Camera camera = parent.GetNode<Camera>("Camera");     // 获取相机节点
		Camera2D camera2D = camera.GetNode<Camera2D>("Camera2D"); // 获取Camera2D节点
		camera2D.Position = new Vector2(1600, 1600);          // 设置相机位置
		camera2D.Zoom = new Vector2(0.1f, 0.1f);              // 设置相机缩放
	}
	public float[,,] MapData; // 三维数组，存储地图的噪声数据
							  // 生成地图
	public void MakeMap()
	{
		setMapData(); // 生成地图数据
		setLayer();   // 创建并设置地图层级
	}
	// 生成地图的噪声数据
	public void setMapData()
	{
		MapData = new float[MapSize.X, MapSize.Y, ZNumber]; // 初始化数组
		for (int x = 0; x < MapSize.X; x++)
		{
			for (int y = 0; y < MapSize.Y; y++)
			{
				for (int z = 0; z < ZNumber; z++)
				{


					MapData[x, y, z] = noise.GetNoise3D(x, y, z * 3); // 获取噪声值
				}
			}
		}
	}
	// 创建并设置地图的层级
	public void setLayer()
	{
		Node2D node2D = GetNode<Node2D>("Node2D"); // 获取地图节点
		for (int x = 0; x < ZNumber; x++)
		{
			// 加载TileMapLayer预制体并实例化
			PackedScene tileMapLayer = GD.Load<PackedScene>("res://Com/PlayerMap/tile_map_layer.tscn");
			var tileMapLayerInstance = tileMapLayer.Instantiate<TileMapLayer>();
			tileMapLayerInstance.Name = "" + (x - ZNumber / 2); // 设置层级名称
			node2D.AddChild(tileMapLayerInstance); // 将层级添加到地图节点
		}
		setLayer0(); // 设置层级的瓦片
	}

	// 为每个层级设置瓦片
	private void setLayer0()
	{

		Random random = new Random();
		for (int z = 0; z < ZNumber; z++)
		{
			TileMapLayer layer0 = node2D.GetChild<TileMapLayer>(z); // 获取层级节点
			for (int x = 0; x < MapSize.X; x++)
			{
				for (int y = 0; y < MapSize.Y; y++)
				{
					float a = MapData[x, y, z]; // 获取对应位置的噪声值
					Vector2I cellPosition = new Vector2I(x, y); // 当前瓦片的位置
					if (a >= 0 && a <= 1)
					{
						// 0-2的随机数



						// 根据噪声值设置瓦片索引
						layer0.SetCell(cellPosition, 1, new Vector2I(random.Next(0, 10), 0));
					}
					else
					{


						layer0.SetCell(cellPosition, 2, new Vector2I(random.Next(0, 10), 0));


					}
				}
			}
		}
		SetTree();
	}

	public void SetTree()
	{
		// 确保路径大小写正确
		PackedScene tree = GD.Load<PackedScene>("res://Com/PlayerMap/Tree/tree.tscn");
		var treeInstance = tree.Instantiate<Tree>();
		MarkeMap_tree mt = new MarkeMap_tree();

		treeInstance.Setup(mt.getTreePath("Oak"));
		// treeInstance.Setup(mt.getTreePath("Cedar"));
		GD.Print(mt.getTreePath("Oak").TreeClassName);
		AddChild(treeInstance);
	}
}
