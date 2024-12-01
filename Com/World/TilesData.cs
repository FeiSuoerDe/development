using System;
using Godot;

/// <summary>
/// 地块数据,包含名称、海拔、湿度、年降雨量、野生动物数、地块类型（水域、泥土、草原）、地下水含量、植被密度
/// </summary>
public class TilesData
{

    public Vector2I pos;
    // 地块名称
    public string Name { get; set; }

    // 地块类型
    public string TileClass { get; set; }

    // 地块海拔
    public float Elevation { get; set; }

    // 夏季温度
    public float SummerTemperature { get; set; }

    // 冬季温度
    public float WinterTemperature { get; set; }

    // 年降雨量
    public float AnnualRainfall { get; set; }

    // 地块湿度
    public float Humidity { get; set; }

    // 地下水含量
    public float GroundwaterContent { get; set; }

    // 野生动物数量
    public int WildlifeCount { get; set; }

    // 植被密度
    public float VegetationDensity { get; set; }

    /// <summary>
    /// 默认构造方法，初始化属性为默认值
    /// </summary>
    public TilesData()
    {

    }

    private String[] tileClass ={
        "深海",
        "海洋",
        "浅滩",
        "平原",
        "高地",
        "裸石平原",
        "高山",
        "顶峰雪原"

    };

    /// <summary>
    /// 带参数的构造方法，使用指定值初始化属性
    /// </summary>
    public TilesData(float elevation, float humidity, float annualRainfall, int wildlifeCount, float groundwaterContent, float vegetationDensity)
    {
        Name = "";
        Elevation = elevation;
        Humidity = humidity;
        AnnualRainfall = annualRainfall;
        WildlifeCount = wildlifeCount;
        GroundwaterContent = groundwaterContent;
        VegetationDensity = vegetationDensity;
    }

    /// <summary>
    /// 根据类型索引设置地块类型
    /// </summary>
    /// <param name="type">地块类型的索引</param>
    public void setTileClass(int type)
    {
        TileClass = tileClass[type];
    }

    /// <summary>
    /// 返回对象的字符串表示形式
    /// </summary>
    /// <returns>包含所有属性的字符串</returns>
    public override string ToString()
    {
        return $"名称: {Name}, 地块类型: {TileClass}, 海拔: {Elevation}, 湿度: {Humidity}, 年降雨量: {AnnualRainfall}, 野生动物数量: {WildlifeCount}, 地下水含量: {GroundwaterContent}, 植被密度: {VegetationDensity}";
    }

    private Random rand = new Random();

    /// <summary>
    /// 生成指定范围内的随机浮点数
    /// </summary>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns>随机浮点数</returns>
    private float GetRandomFloat(float min, float max)
    {
        return (float)(rand.NextDouble() * (max - min) + min);
    }

    /// <summary>
    /// 根据逻辑为属性赋值
    /// </summary>
    public void AssignValuesBasedOnLogic()
    {
        int summerBase, winterBase;

        // 根据海拔高度设置基准温度
        if (Elevation > 70)
        {
            summerBase = 15;
            winterBase = -5;
        }
        else if (Elevation > 50)
        {
            summerBase = 20;
            winterBase = 0;
        }
        else
        {
            summerBase = 25;
            winterBase = 5;
        }

        // 生成温度的随机波动值
        int summerTempFluctuation = rand.Next(-2, 3); // 夏季温度波动范围：-2 到 +2
        int winterTempFluctuation = rand.Next(-1, 2); // 冬季温度波动范围：-1 到 +1

        // 根据地块类型生成降雨量和湿度的波动值
        int rainfallFluctuation = TileClass == "森林" ? rand.Next(-100, 101) :
                                  TileClass == "沙漠" ? rand.Next(-20, 21) :
                                  rand.Next(-50, 51);

        int humidityFluctuation = TileClass == "森林" ? rand.Next(-5, 6) :
                                  TileClass == "沙漠" ? rand.Next(-2, 3) :
                                  rand.Next(-3, 4);

        // 生成其他属性的随机波动值
        int groundwaterFluctuation = rand.Next(-5, 6);
        int wildlifeFluctuation = rand.Next(-10, 11);
        int vegetationFluctuation = rand.Next(-5, 6);

        // 设置温度属性
        SummerTemperature = summerBase + summerTempFluctuation;
        WinterTemperature = winterBase + winterTempFluctuation;

        // 设置降雨量和湿度属性
        switch (TileClass)
        {
            case "森林":
                AnnualRainfall = 1500 + rainfallFluctuation;
                Humidity = 80 + humidityFluctuation;
                break;
            case "沙漠":
                AnnualRainfall = 100 + rainfallFluctuation;
                Humidity = 20 + humidityFluctuation;
                break;
            default:
                AnnualRainfall = 800 + rainfallFluctuation;
                Humidity = 50 + humidityFluctuation;
                break;
        }

        // 设置地下水含量、野生动物数量和植被密度属性
        GroundwaterContent = (TileClass == "沙漠" ? 10 : 50) + groundwaterFluctuation;
        WildlifeCount = (TileClass == "森林" ? 100 : 20) + wildlifeFluctuation;
        VegetationDensity = (TileClass == "森林" ? 70 : 30) + vegetationFluctuation;
    }
}