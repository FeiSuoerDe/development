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

    public string toStringTree()
    {
        return tree.ToString();
    }
}
