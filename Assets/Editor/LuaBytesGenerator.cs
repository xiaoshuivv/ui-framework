using System.IO;
using UnityEditor;
using UnityEngine;

public class LuaBytesGenerator : EditorWindow
{
    private string luaSourcePath = "Assets/Lua";
    private string luaBytesOutputPath = "Assets/Resources/LuaBytes";
    
    [MenuItem("Tools/XLua/生成Lua字节码")]
    public static void ShowWindow()
    {
        GetWindow<LuaBytesGenerator>("Lua字节码生成器");
    }
    
    void OnGUI()
    {
        GUILayout.Label("Lua字节码生成设置", EditorStyles.boldLabel);
        
        luaSourcePath = EditorGUILayout.TextField("Lua源文件路径:", luaSourcePath);
        luaBytesOutputPath = EditorGUILayout.TextField("字节码输出路径:", luaBytesOutputPath);
        
        if (GUILayout.Button("生成Lua字节码"))
        {
            GenerateLuaBytes();
        }
    }
    
    private void GenerateLuaBytes()
    {
        if (!Directory.Exists(luaSourcePath))
        {
            Debug.LogError($"Lua源文件路径不存在: {luaSourcePath}");
            return;
        }
        
        if (!Directory.Exists(luaBytesOutputPath))
        {
            Directory.CreateDirectory(luaBytesOutputPath);
        }
        
        string[] luaFiles = Directory.GetFiles(luaSourcePath, "*.lua", SearchOption.AllDirectories);
        
        foreach (string luaFile in luaFiles)
        {
            string relativePath = GetRelativePath(luaFile, luaSourcePath);
            string outputPath = Path.Combine(luaBytesOutputPath, relativePath + ".bytes");
            
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
            
            byte[] luaBytes = File.ReadAllBytes(luaFile);
            File.WriteAllBytes(outputPath, luaBytes);
            
            Debug.Log($"生成Lua字节码: {relativePath} -> {outputPath}");
        }
        
        AssetDatabase.Refresh();
        Debug.Log("Lua字节码生成完成!");
    }
    
    private string GetRelativePath(string filePath, string basePath)
    {
        filePath = filePath.Replace('\\', '/');
        basePath = basePath.Replace('\\', '/');
        
        if (filePath.StartsWith(basePath))
        {
            return filePath.Substring(basePath.Length + 1).Replace(".lua", "");
        }
        
        return filePath;
    }
}