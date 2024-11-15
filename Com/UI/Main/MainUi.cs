
using Godot;
using System;

public partial class MainUi : Control
{
	[Export]
	public Button Start;
	[Export]
	public Button Quit;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		Start.Pressed += StartFun;
		Quit.Pressed += QuitFun;
	}

	private void StartFun()
	{
		GetTree().ChangeSceneToFile("res://Com/Main/main.tscn");
	}
	private void QuitFun()
	{
		GetTree().Quit();
	}
}
