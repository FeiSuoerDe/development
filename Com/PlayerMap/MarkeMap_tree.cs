using System;
using System.Text.Json;

public partial class MarkeMap_tree
{

    public MarkeMap_tree()
    {
        MarkeTree();
        AssignValues();
    }
    public Tree tree = new Tree();
    String jsonStr;
    public void MarkeTree()
    {
        jsonStr = new JsonTool().getJsonDataToString(@".\.\.\Data\Json\Tree\Tree_Oak.json");
    }





    // 赋值
    public void AssignValues()
    {
        // tree.ClassName = ExtractAttributes("TreeClassName");
        tree = JsonSerializer.Deserialize<Tree>(jsonStr);

    }
    // 提取属性
    // public string ExtractAttributes(String name)
    // {
    //     using (JsonDocument doc = JsonDocument.Parse(jsonStr))
    //     {
    //         JsonElement root = doc.RootElement;
    //         if (root.TryGetProperty(name, out JsonElement value))
    //         {
    //             // 处理提取的值
    //             return $"{name}: {value}";
    //         }
    //         else
    //         {
    //             return $"未找到名称为 '{name}' 的属性。";
    //         }
    //     }
    // }

}