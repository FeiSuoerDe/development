using System;
using System.Security.Cryptography.X509Certificates;
using Godot;

public partial class PlayerMap : Node
{
	[Export]
	public Node2D node2D; // 地图节点，包含所有的 TileMapLayer

	[Export]
	public FastNoiseLite noise { get; set; } // 用于生成噪声数据的对象

	public TilesData tilesData { get; set; } // 地块数据，包含地形和环境信息

	public Vector2I MapSize = new Vector2I(100, 100); // 地图尺寸（宽度，高度）
	NodeController nodeController = new NodeController(); // 场景节点控制器

	[Export]
	public int ZNumber = 1; // 地图的层数（深度）


	public override void _Ready()
	{
		MakeMap();        // 生成地图数据和层级
		setCamera();      // 设置相机位置和缩放
	}

	// 设置相机位置和缩放
	public void setCamera()
	{
		var parent = GetParent();                // 获取父节点
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
				MapData[x, y, 0] = noise.GetNoise3D(x, y, 0); // 获取噪声值
			}
		}
	}

	// 创建并设置地图的层级
	public void setLayer()
	{
		Node2D node2D = GetNode<Node2D>("Node2D"); // 获取地图节点
		PackedScene tileMapLayer = GD.Load<PackedScene>("res://Com/PlayerMap/tile_map_layer.tscn"); // 加载TileMapLayer预制体

		var tileMapLayerInstance = tileMapLayer.Instantiate<TileMapLayer>();
		tileMapLayerInstance.Name = "0"; // 设置层级名称
		node2D.AddChild(tileMapLayerInstance); // 将层级添加到地图节点

	}
}

