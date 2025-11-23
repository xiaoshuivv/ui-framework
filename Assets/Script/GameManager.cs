using UnityEngine;
using XLua;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;
    
    private LuaEnv luaEnv;
    private GameObject uiRoot;
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitUIRoot();
            InitLuaEnv();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitUIRoot()
    {
        // 加载UIRoot预制体
        GameObject uiRootPrefab = Resources.Load<GameObject>("UI/Prefabs/UIRoot");
        
        if (uiRootPrefab != null)
        {
            uiRoot = Instantiate(uiRootPrefab);
            DontDestroyOnLoad(uiRoot);
            
            // 确保UIManager存在
            if (UIManager.Instance == null)
            {
                GameObject uiManagerObj = new GameObject("UIManager");
                uiManagerObj.transform.SetParent(uiRoot.transform);
                uiManagerObj.AddComponent<UIManager>();
            }
            
            Debug.Log("UIRoot初始化完成");
        }
        else
        {
            Debug.LogError("UIRoot预制体加载失败: UI/Prefabs/UIRoot");
        }
    }
    
    private void InitLuaEnv()
    {
        luaEnv = new LuaEnv();
        luaEnv.AddLoader(CustomLoader);
        
        // 加载基础Lua脚本
        luaEnv.DoString("require 'StartGame'");
    }
    
    private byte[] CustomLoader(ref string filepath)
    {
        string resourcesPath = "LuaBytes/" + filepath;
        TextAsset luaBytes = Resources.Load<TextAsset>(resourcesPath);
        
        if (luaBytes != null)
        {
            Debug.Log($"成功加载Lua文件: {resourcesPath}");
            return luaBytes.bytes;
        }
        
        Debug.LogError($"Lua文件加载失败: {filepath}，请检查路径：{resourcesPath}");
        return null;
    }
    
    public void DoLuaScript(string scriptPath, GameObject bindObject = null)
    {
        try
        {
            luaEnv.DoString($"require '{scriptPath}'");
            
            if (bindObject != null)
            {
                LuaTable luaTable = luaEnv.Global.Get<LuaTable>(scriptPath);
                if (luaTable != null)
                {
                    luaTable.Set("gameObject", bindObject);
                    luaTable.Set("transform", bindObject.transform);
                    
                    // 注册到UIManager
                    UIManager.Instance.RegisterLuaPanel(scriptPath, luaTable);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Lua脚本执行失败 {scriptPath}: {ex.Message}");
        }
    }
    
    public LuaTable GetLuaTable(string tableName)
    {
        return luaEnv.Global.Get<LuaTable>(tableName);
    }
    
    void Update()
    {
        if (luaEnv != null)
        {
            luaEnv.Tick();
        }
    }
    
    void OnDestroy()
    {
        luaEnv?.Dispose();
    }
}
