using Godot;
using System;
using System.Data;
using System.Dynamic;

public partial class PlayerMap : Node
{

	[Export]
	public Node2D node2D;
	// 游玩地图
	[Export]
	public FastNoiseLite noise { get; set; }
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
	public TilesData tilesData { get; set; }

	public Vector2I MapSize = new Vector2I(784, 784);
	[Export]
	public int ZNumber = 20;
	public override void _Ready()
	{
		// Called every time the node is added to the scene.
		// Initialization here
		MakeMap();
		setCamera();
	}
	public void setCamera()
	{
		Node2D parent = GetParent() as Node2D; // 获取父节点
		Camera camera = parent.GetNode<Camera>("Camera"); // 获取世界节点
		camera.GetNode<Camera2D>("Camera2D").Position = new Vector2(6272, 6272);
		camera.GetNode<Camera2D>("Camera2D").Zoom = new Vector2(0.1f, 0.1f);
	}
	public float[,,] MapData;
	// 生成地图
	public void MakeMap()
	{
		setMapData();
		setLayer();
	}
	public void setMapData()
	{
		MapData = new float[MapSize.X, MapSize.Y, ZNumber];
		for (int x = 0; x < MapSize.X; x++)
		{
			for (int y = 0; y < MapSize.Y; y++)
			{
				for (int z = 0; z < ZNumber; z++)
				{
					MapData[x, y, z] = noise.GetNoise3D(x, y, z);
				}
			}
		}
	}
	public void setLayer()
	{
		Node2D node2D = GetNode<Node2D>("Node2D");
		for (int x = 0; x < ZNumber; x++)
		{
			PackedScene tileMapLayer = GD.Load<PackedScene>("res://Com/PlayerMap/tile_map_layer.tscn");
			var tileMapLayerInstance = tileMapLayer.Instantiate<TileMapLayer>();
			tileMapLayerInstance.Name = "Layer" + x;
			node2D.AddChild(tileMapLayerInstance);

		}
	}






}
