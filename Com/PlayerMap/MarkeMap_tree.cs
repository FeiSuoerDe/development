using Godot;
using System;
using System.Collections.Generic;
using System.Text.Json;

public partial class MarkeMap_tree
{
    // JSON文件路径
    public string TreesPath = @".\.\.\Data\Json\Tree\TreeS.json";
    // JSON文件内容字符串
    public string TreesStr;
    // 树木配置对象
    public TreeCfg Oaktree;
    public TreeCfg Cedartree;
    // 临时存储JSON字符串
    private string jsonStr;
    // 树木路径对象
    public TreePath treePath;

    // 树木路径类
    public class TreePath
    {
        public string Tree_Oak_Path { get; set; }
        public string Tree_Cedar_Path { get; set; }
    }

    // 树木名称与路径字典
    private Dictionary<string, string> TreePathDic = new Dictionary<string, string>();

    // 构造函数，初始化树木路径字典
    public MarkeMap_tree()
    {
        // 读取JSON文件内容
        TreesStr = new JsonTool().getJsonDataToString(TreesPath);
        // 反序列化JSON字符串为TreePath对象
        treePath = JsonSerializer.Deserialize<TreePath>(TreesStr);
        GD.Print(treePath.Tree_Oak_Path);
        // 添加树木路径到字典
        TreePathDic.Add("Oak", treePath.Tree_Oak_Path);
        TreePathDic.Add("Cedar", treePath.Tree_Cedar_Path);
    }

    // 读取指定树木的JSON文件
    public void MarkeTree(string treeName)
    {
        jsonStr = new JsonTool().getJsonDataToString(treeName);
    }

    // 将JSON字符串反序列化为树木配置对象
    public void AssignValues()
    {
        Oaktree = JsonSerializer.Deserialize<TreeCfg>(jsonStr);
    }
}
