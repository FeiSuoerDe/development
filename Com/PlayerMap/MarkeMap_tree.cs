using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class MarkeMap_tree
{
    public MarkeMap_tree()
    {
        setTreeS();
    }
    //用于存放各种树的路径 
    public string TreeSPath = @".\.\.\Data\Json\Tree\TreeS.json";
    public class TreePath
    {
        public string Tree_Oak_Path { get; set; }
        public string Tree_Cedar_Path { get; set; }
    }
    private Dictionary<string, string> TreePathDic = new Dictionary<string, string>();
    public void setTreeS()
    {
        // 读取JSON文件内容
        string TreesStr = new JsonTool().getJsonDataToString(TreeSPath);
        // 反序列化JSON字符串为TreePath对象
        TreePath treePath = JsonSerializer.Deserialize<TreePath>(TreesStr);
        TreePathDic.Add("Oak", treePath.Tree_Oak_Path);
        TreePathDic.Add("Cedar", treePath.Tree_Cedar_Path);
    }
    public TreeCfg treeConfig;
    public TreeCfg getTreePath(string treeName)
    {
        if (!TreePathDic.ContainsKey(treeName))
        {
            GD.PrintErr($"Tree path not found for: {treeName}");
            return null;
        }

        string jsonStr = new JsonTool().getJsonDataToString(TreePathDic[treeName]);
        return treeConfig = JsonSerializer.Deserialize<TreeCfg>(jsonStr);
    }



}
