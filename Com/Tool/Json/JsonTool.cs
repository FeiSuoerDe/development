using System;
using System.IO;

public class JsonTool
{

    public string JsonPath;  // json文件的路径

    public string getJsonDataToString(String path)
    {
        string jsonStr = "";
        jsonStr = File.ReadAllText(path);
        return jsonStr;
    }
}