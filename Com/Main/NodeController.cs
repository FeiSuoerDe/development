using Godot;
using System;

public partial class NodeController : Node
{


	// 返回实例化的节点
	public Node GetNodeInstance(string path)
	{
		PackedScene scene = GD.Load<PackedScene>(path);
		if (scene == null)
		{
			GD.PrintErr($"无法加载场景：{path}");
			return null;
		}
		return scene.Instantiate();

	}

}
