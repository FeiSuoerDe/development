using Godot;
using System;
using System.Text.Json;

public partial class Tree : Node2D
{
    [Export]
    public Sprite2D sprite2D { get; set; }  // 树的精灵节点，用于呈现树的图像
    public CompressedTexture2D compressedTexture2D { get; set; }  // 压缩纹理，用于加载树的纹理资源

    // 物理属性
    // 类名
    public string TreeClassName { get; set; }   // 类名，用于标识树的类型
    public string Color { get; set; }       // 颜色，表示树的颜色
    public float Density { get; set; }      // 密度，影响树的物理碰撞和重量
    public float MaxHeight { get; set; }    // 最大高度，树能生长到的最大高度
    public float CurrentHeight { get; set; } // 当前高度，树当前的实际高度

    // 生长属性
    public float HP { get; set; }                // 生命值，表示树的健康状态
    public float MoistureContent { get; set; }   // 水分含量，影响树的生长速度
    public float EvaporationRate { get; set; }   // 蒸发速率，水分流失的速度
    public float DeathLowTemperature { get; set; }  // 最低致死温度，低于此温度树会死亡
    public float DeathHighTemperature { get; set; } // 最高致死温度，高于此温度树会死亡
    public float GrowthSpeed { get; set; }       // 生长速度，影响树的生长速率
    public float[] SuitableRange { get; set; }   // 适宜生长的温度范围

    // 生长阶段枚举
    public enum GrowthStages
    {
        Seedling = 1,    // 幼苗阶段
        MatureTree = 2   // 成熟树阶段
    }
    public string Sprite2DImgPath;

    // 动画属性
    public int AnimationH { get; set; }  // 动画横向帧数（列数）
    public int AnimationV { get; set; }  // 动画纵向帧数（行数）
    public int AnimationF { get; set; }  // 动画当前帧数

    // 2D贴图属性
    public string SpritePath { get; set; }  // 精灵贴图的资源路径

    // jsonPath
    public string JsonPath { get; set; }  // json文件的路径
    public void LoadDataFromJson()
    {

    }
    public void SetSprite2D()
    {
        // 初始化压缩纹理并加载纹理资源
        compressedTexture2D = new CompressedTexture2D();
        compressedTexture2D.Load(Sprite2DImgPath);

        // 设置精灵的动画帧参数
        sprite2D.Hframes = AnimationH;  // 设置横向帧数
        sprite2D.Vframes = AnimationV;  // 设置纵向帧数
        sprite2D.Frame = AnimationF;    // 设置当前帧
    }
    public override void _Ready()
    {

        SetSprite2D();
        LoadDataFromJson();
    }
    public String ToString()
    {
        return $"TreeClassName: {TreeClassName}, Color: {Color}, Density: {Density}, MaxHeight: {MaxHeight}, CurrentHeight: {CurrentHeight}, HP: {HP}, MoistureContent: {MoistureContent}, EvaporationRate: {EvaporationRate}, DeathLowTemperature: {DeathLowTemperature}, DeathHighTemperature: {DeathHighTemperature}, GrowthSpeed: {GrowthSpeed}, SuitableRange: {SuitableRange}, AnimationH: {AnimationH}, AnimationV: {AnimationV}, AnimationF: {AnimationF}, SpritePath: {SpritePath}, JsonPath: {JsonPath}";
    }

}
