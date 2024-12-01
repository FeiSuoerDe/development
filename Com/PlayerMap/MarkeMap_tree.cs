class MarkeMap_tree
{
    public string MarkeTree()
    {
        return new JsonTool().getJsonDataToString(@".\.\.\Data\Json\Tree\TreeS-CN.json");
    }

}