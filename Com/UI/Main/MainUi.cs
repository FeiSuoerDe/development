
using Godot;


public partial class MainUi : Control
{
	[Export]
	public Button Start;
	[Export]
	public Button Quit;
	public override void _Ready()
	{


		if (Start != null)
		{

			Start.Pressed += StartFun;
		}
		else
		{
			GD.Print("启动组件丢失");
		}
		if (Quit != null)
		{

			Quit.Pressed += QuitFun;
		}
		else
		{
			GD.Print("退出方法缺失");
		}
	}

	// 启动
	private void StartFun()
	{
		var parent = GetParent();

		// 添加相机组件
		PackedScene cameraScene = GD.Load<PackedScene>("res://Com/Camera/camera.tscn"); // 修改为实际文件名
		if (cameraScene != null)
		{
			Camera cameraInstance = cameraScene.Instantiate<Camera>();
			parent.AddChild(cameraInstance);
		}
		else
		{
			GD.PrintErr("无法加载相机场景：res://Com/Camera/camera.tscn");
		}

		// 添加World组件
		PackedScene worldScene = GD.Load<PackedScene>("res://Com/World/World.tscn");
		if (worldScene != null)
		{
			World worldInstance = worldScene.Instantiate<World>();
			parent.AddChild(worldInstance);
		}
		else
		{
			GD.PrintErr("无法加载世界场景：res://Com/World/World.tscn");
		}
		// 添加worldUI组件
		PackedScene worldUIScene = GD.Load<PackedScene>("res://Com/UI/WorldUI/world_ui.tscn");
		if (worldUIScene != null)
		{
			WorldUi worldUIInstance = worldUIScene.Instantiate<WorldUi>();
			parent.AddChild(worldUIInstance);
		}
		else
		{
			GD.PrintErr("无法加载世界UI场景：res://Com/UI/WorldUI/WorldUI.tscn");
		}
		// 删除自身
		QueueFree();
	}
	// 退出
	private void QuitFun()
	{
		// 保存数据
		GetTree().Quit();
	}
}
