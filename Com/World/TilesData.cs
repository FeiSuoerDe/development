class TilesData
{
    // 地块数据,包含名称海拔湿度，年降雨量，野生动物数，地块类型（水域泥土草原），地下水含量，植被密度
    // 添加注释以解释每个字段的含义
    
    public string Name { get; set; } // 地块名称
    public float Elevation { get; set; } // 地块海拔
    public float Humidity { get; set; } // 地块湿度
    public float AnnualRainfall { get; set; } // 年降雨量
    public int WildlifeCount { get; set; } // 野生动物数量
    public string TerrainType { get; set; } // 地块类型（水域、泥土、草原）
    public float GroundwaterContent { get; set; } // 地下水含量
    public float VegetationDensity { get; set; } // 植被密度
    // 构造方法
    public TilesData(string name, float elevation, float humidity, float annualRainfall, int wildlifeCount, string terrainType, float groundwaterContent, float vegetationDensity)
    {
        Name = name;
        Elevation = elevation;
        Humidity = humidity;
        AnnualRainfall = annualRainfall;
        WildlifeCount = wildlifeCount;
        TerrainType = terrainType;
        GroundwaterContent = groundwaterContent;
        VegetationDensity = vegetationDensity;
    }
    // public void toString()
    // {
    //     GD.Print($"地块名称：{Name}, 海拔：{Elevation}, 湿度：{Humidity}, 年降雨量：{AnnualRainfall}, 野生动物数量：{WildlifeCount}, 地块类型：{TerrainType}, 地下水含量：{GroundwaterContent}, 植被密度：{VegetationDensity}");
    // }
}