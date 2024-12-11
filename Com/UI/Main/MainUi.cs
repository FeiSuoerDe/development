
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
	NodeController nodeController;

	// 启动
	private void StartFun()
	{



		var parent = GetParent();
		nodeController = parent.GetParent().GetNode<NodeController>("NodeController");

		// 添加相机组件
		nodeController.AddNode(parent, nodeController.GetNodeInstance("camera"));

		// 添加World组件
		nodeController.AddNode(parent, nodeController.GetNodeInstance("world"));
		// 添加worldUI组件
		nodeController.AddNode(parent, nodeController.GetNodeInstance("world_ui"));
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
