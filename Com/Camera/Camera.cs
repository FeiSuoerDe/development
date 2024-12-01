using Godot;
using System;

public partial class Camera : Node
{
    [Export]
    public Camera2D camera2D; // 引用Camera2D节点

    [Export]
    public float BaseSpeed = 300f; // 基础移动速度

    private Vector2 movement = Vector2.Zero; // 移动向量
    private const float ZoomFactor = 1.2f; // 缩放系数

    public override void _Process(double delta)
    {
        float currentSpeed = BaseSpeed / camera2D.Zoom.X; // 根据缩放调整当前速度

        movement = Vector2.Zero; // 重置移动向量

        // 获取输入，更新移动方向
        movement.X = Input.IsActionPressed("d") ? 1 : (Input.IsActionPressed("a") ? -1 : 0);
        movement.Y = Input.IsActionPressed("s") ? 1 : (Input.IsActionPressed("w") ? -1 : 0);

        if (movement != Vector2.Zero)
        {
            movement = movement.Normalized(); // 规范化移动向量
            camera2D.Position += movement * currentSpeed * (float)delta; // ���新相机位置
        }
        // 缩小视图
        if (Input.IsActionJustPressed("mous_up"))
        {
            camera2D.Zoom *= ZoomFactor;
        }
        // 放大视图
        if (Input.IsActionJustPressed("mous_down"))
        {
            camera2D.Zoom /= ZoomFactor;
        }
    }
}