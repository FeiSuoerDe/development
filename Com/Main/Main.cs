using Godot;
using System;

public partial class Main : Node2D
{
	// 节点准备就绪时调用
	public override void _Ready()
	{
		// 加载主界面场景资源
		PackedScene mainUi = GD.Load<PackedScene>("res://Com/UI/Main/main_ui.tscn");
		// 实例化主界面
		MainUi mainUiInstance = mainUi.Instantiate() as MainUi;
		// 将主界面添加为子节点
		AddChild(mainUiInstance);
	}
}
