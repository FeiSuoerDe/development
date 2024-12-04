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
    public string TreeClassName { get; set; }
    public PhysicsClass Physics { get; set; }  // 树的物理属性
    // 类名，用于标识树的类型
    public class PhysicsClass
    {
        public string Color { get; set; }       // 颜色，表示树的颜色
        public float Density { get; set; }       // 密度，影响树的物理碰撞和重量
        public HeightClass Height { get; set; }      // 高度，表示树的高度
        public class HeightClass
        {
            public float MaxHeight { get; set; }    // 最大高度，树能生长到的最大高度
            public float CurrentHeight { get; set; } // 当前高度，树当前的实际高度
        }

    }

    public GrowthAttributesClass GrowthAttributes { get; set; }
    // 生长属性
    public class GrowthAttributesClass
    {
        public float HP { get; set; }                // 生命值，表示树的健康状态
        public float MoistureContent { get; set; }   // 水分含量，影响树的生长速度
        public float EvaporationRate { get; set; }   // 蒸发速率，水分流失的速度
        public float DeathLowTemperature { get; set; }  // 最低致死温度，低于此温度树会死亡
        public float DeathHighTemperature { get; set; } // 最高致死温度，高于此温度树会死亡
        public float GrowthSpeed { get; set; }       // 生长速度，影响树的生长速率
        public GrowthStagesClass GrowthStages { get; set; }  // 生长阶段
        public class GrowthStagesClass
        {
            public int Seedling { get; set; }    // 幼苗阶段
            public int MatureTree { get; set; }  // 成熟树阶段
        }
        public float[] SuitableRange { get; set; }   // 适宜生长的温度范围
    }

    public Sprite2DClass Sprite2D { get; set; }  // 精灵2D
    public class Sprite2DClass
    {
        public string Path { get; set; }
        public AnimationClass Animation { get; set; }
        public class AnimationClass
        {
            public int H { get; set; }
            public int V { get; set; }
            public int F { get; set; }
        }
    }



    // 2D贴图属性
    public string SpritePath { get; set; }  // 精灵贴图的资源路径

    // jsonPath
    public string JsonPath { get; set; }  // json文件的路径

    public void SetSprite2D()
    {
        // 初始化压缩纹理并加载纹理资源
        compressedTexture2D = new CompressedTexture2D();
        compressedTexture2D.Load(SpritePath);

        // 设置精灵的动画帧参数
        sprite2D.Hframes = Sprite2D.Animation.H;  // 设置横向帧数
        sprite2D.Vframes = Sprite2D.Animation.V;  // 设置纵向帧数
        sprite2D.Frame = Sprite2D.Animation.F;    // 设置当前帧
    }
    public override void _Ready()
    {

        SetSprite2D();
    }

}
