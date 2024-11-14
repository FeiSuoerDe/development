using Godot;
using System;

public partial class Camera : Node
{
	[Export]
	public Camera2D camera2D;

	private float baseSpeed = 300;
	private Vector2 movement = new Vector2();

	public override void _Process(double delta)
	{
		// 计算当前速度，使其与缩放级别成反比
		float currentSpeed = baseSpeed / camera2D.Zoom.X;

		movement = new Vector2(0, 0); // 重置移动向量

		// 检查方向键并更新移动向量
		if (Input.IsActionPressed("w")) movement += new Vector2(0, -1);
		if (Input.IsActionPressed("s")) movement += new Vector2(0, 1);
		if (Input.IsActionPressed("a")) movement += new Vector2(-1, 0);
		if (Input.IsActionPressed("d")) movement += new Vector2(1, 0);

		// 更新相机位置
		camera2D.Position += movement * currentSpeed * (float)delta;

		// 处理鼠标滚轮缩放
		if (Input.IsActionJustPressed("mous_up"))
		{
			camera2D.Zoom *= 1.2f;
		}
		if (Input.IsActionJustPressed("mous_down"))
		{
			camera2D.Zoom /= 1.2f;
		}
	}
}