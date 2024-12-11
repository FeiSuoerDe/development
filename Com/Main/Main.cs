using Godot;
using System;

public partial class Main : Node2D
{

	[Export]
	public Ui UI;
	[Export]
	public Music Music;

	[Export]
	public NodeController NodeController;


	public override void _Ready()
	{
		NodeController.AddNode(UI, NodeController.GetNodeInstance("main_ui"));
	}
}
