using Godot;
using System;

public partial class WorldUi : Control
{
	// 当节点第一次进入场景树时调用
	[Export]
	public Label label; // 显示文本
	[Export]
	public Button Stard; // 开始按钮
	[Export]
	public Button Again; // 重来按钮
	[Export]
	public AnimationPlayer animationPlayer; // 动画播放器

	[Export]
	public Sprite2D sprite2D; // 2D精灵
	public TilesData tileData; // 瓦片数据
	public override void _Ready()
	{
		Stard.Pressed += StartFun; // 绑定开始按钮的点击事件

		Again.Pressed += AgainFun; // 绑定重来按钮的点击事件
		Stard.Disabled = true; // 禁用开始按钮
		animationPlayer.Play("Aspr"); // 播放动画
	}

	public void setLabel()
	{
		label.Text = tileData.ToString(); // 设置标签文本为瓦片数据的字符串表示
	}
	private Node2D father;
	NodeController nodeController;
	private void StartFun()
	{

		// 		E 0:00:12:0336   void WorldUi.StartFun(): System.NullReferenceException: Object reference not set to an instance of an object.
		//   < C# 错误>        System.NullReferenceException
		//   < C# 源文件>       WorldUi.cs:38 @ void WorldUi.StartFun()
		//   < 栈追踪 > WorldUi.cs:38 @ void WorldUi.StartFun()

		// 				 Callable.generics.cs:39 @ void Godot.Callable.< From > g__Trampoline | 1_0(object, Godot.NativeInterop.NativeVariantPtrArgs, Godot.NativeInterop.godot_variant &)

		// 				 DelegateUtils.cs:86 @ void Godot.DelegateUtils.InvokeWithVariantArgs(nint, System.Void *, Godot.NativeInterop.godot_variant * *, int, Godot.NativeInterop.godot_variant *)


		Node2D parent = GetParent() as Node2D; // 获取父节点
		nodeController = parent.GetParent().GetNode<NodeController>("NodeController");
		// PackedScene PlayerMap = GD.Load<PackedScene>("res://Com/PlayerMap/player_map.tscn"); // 加载玩家地图场景
		PlayerMap playerMap = nodeController.GetNodeInstance("player_map") as PlayerMap; // 获取玩家地图实例
		nodeController.AddNode(parent, playerMap); // 添加玩家地图



		playerMap.tilesData = tileData; // 设置玩家地图的瓦片数据


		World world = parent.GetNode<World>("World"); // 获取世界节点
		playerMap.noise = world.NoiseGenerator;
		playerMap.noise.Seed = world.NoiseGenerator.Seed; // 设置噪声生成器

		world.QueueFree(); // 释放世界节点
		parent.GetNode<WorldUi>("WorldUI").QueueFree(); // 释放世界UI节点
	}

	public void setSprite(Vector2 vector2)
	{
		sprite2D.Position = vector2; // 设置精灵的位置
	}
	private void AgainFun()
	{
		Node2D parent = GetParent() as Node2D;

		World world = parent.GetNode<World>("World");
		world.SetTerrain(); // 重置地形
		label.Text = ""; // 清空标签文本
	}
}
