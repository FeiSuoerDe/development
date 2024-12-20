using Godot;
using System;

using System.Collections.Generic;

/// <summary>
/// 管理场景节点的控制器类。
/// </summary>
public partial class NodeController : Node
{

	// 地址与场景名字典
	public Dictionary<string, string> SceneDict = new Dictionary<string, string>(){
		{"main_ui", "res://Com/UI/Main/main_ui.tscn"},
		{"camera","res://Com/Camera/camera.tscn"},
		{"world","res://Com/World/World.tscn"},
		{"world_ui","res://Com/UI/WorldUI/world_ui.tscn"},
		{"player_map","res://Com/PlayerMap/player_map.tscn"},
		{"tree",""},
		{"flower",""}

	};

	// 返回实例化的节点
	public PackedScene GetNodeInstance(string Name)
	{
		PackedScene scene = GD.Load<PackedScene>(SceneDict[Name]);
		if (scene == null)
		{
			GD.PrintErr($"无法加载场景：{Name}");
			return null;
		}
		return scene;

	}
	// 添加节点
	public void AddNode(Node parent, PackedScene node)
	{
		if (parent == null)
		{
			GD.PrintErr("父节点为空");
			return;
		}
		if (node == null)
		{
			GD.PrintErr("子节点为空");
			return;
		}
		parent.AddChild(node.Instantiate());
	}
}
