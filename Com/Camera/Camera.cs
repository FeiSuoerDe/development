using Godot;
using System;

public partial class Camera : Node
{
    [Export]
    public Camera2D camera2D;

    [Export]
    public float BaseSpeed = 300f;

    private Vector2 movement = Vector2.Zero;
    private const float ZoomFactor = 1.2f;

    public override void _Process(double delta)
    {
        // 计算当前速度，与缩放级别成反比
        float currentSpeed = BaseSpeed / camera2D.Zoom.X;

        movement = Vector2.Zero; // 重置移动向量

        // 检查方向键并更新移动向量
        movement.X = Input.IsActionPressed("d") ? 1 : (Input.IsActionPressed("a") ? -1 : 0);
        movement.Y = Input.IsActionPressed("s") ? 1 : (Input.IsActionPressed("w") ? -1 : 0);

        if (movement != Vector2.Zero)
        {
            movement = movement.Normalized();
            camera2D.Position += movement * currentSpeed * (float)delta;
        }

        // 处理鼠标滚轮缩放
        if (Input.IsActionJustPressed("mous_up"))
        {
            camera2D.Zoom *= ZoomFactor;
        }
        if (Input.IsActionJustPressed("mous_down"))
        {
            camera2D.Zoom /= ZoomFactor;
        }
    }
}