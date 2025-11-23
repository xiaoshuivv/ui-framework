using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

[LuaCallCSharp]
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance => _instance;
    
    private Dictionary<string, GameObject> loadedPanels = new Dictionary<string, GameObject>();
    private Dictionary<string, LuaTable> luaPanels = new Dictionary<string, LuaTable>();
    private Stack<GameObject> panelStack = new Stack<GameObject>();
    
    [SerializeField]
    private Transform uiRoot;
    
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void OpenPanel(string panelName)
    {
        if (loadedPanels.ContainsKey(panelName))
        {
            BringToTop(panelName);
            return;
        }
        
        // 从UIConfig获取路径
        LuaTable config = GameManager.Instance.GetLuaTable("UIConfig");
        string prefabPath = config.Get<string>(panelName + "_Prefab");
        string luaScriptPath = config.Get<string>(panelName + "_Script");
        
        // 加载预制体
        GameObject panelPrefab = Resources.Load<GameObject>(prefabPath);
        if (panelPrefab == null)
        {
            Debug.LogError($"预制体加载失败: {prefabPath}");
            return;
        }
        
        GameObject panelObj = Instantiate(panelPrefab, uiRoot);
        loadedPanels[panelName] = panelObj;
        panelStack.Push(panelObj);
        
        // 执行Lua脚本
        GameManager.Instance.DoLuaScript(luaScriptPath, panelObj);
        
        // 调用Lua的OnOpen方法
        LuaTable luaTable = luaPanels[panelName];
        if (luaTable != null)
        {
            Action onOpen = luaTable.Get<Action>("OnOpen");
            onOpen?.Invoke();
        }
    }
    
    public void ClosePanel(string panelName)
    {
        if (loadedPanels.TryGetValue(panelName, out GameObject panelObj))
        {
            LuaTable luaTable = luaPanels[panelName];
            Action onClose = luaTable?.Get<Action>("OnClose");
            onClose?.Invoke();
            
            Destroy(panelObj);
            loadedPanels.Remove(panelName);
            luaPanels.Remove(panelName);
            
            // 从栈中移除
            Stack<GameObject> tempStack = new Stack<GameObject>();
            while (panelStack.Count > 0)
            {
                GameObject topPanel = panelStack.Pop();
                if (topPanel != panelObj)
                {
                    tempStack.Push(topPanel);
                }
            }
            while (tempStack.Count > 0)
            {
                panelStack.Push(tempStack.Pop());
            }
        }
    }
    
    private void BringToTop(string panelName)
    {
        if (loadedPanels.TryGetValue(panelName, out GameObject panelObj))
        {
            panelObj.transform.SetAsLastSibling();
            
            Stack<GameObject> tempStack = new Stack<GameObject>();
            while (panelStack.Count > 0)
            {
                GameObject topPanel = panelStack.Pop();
                if (topPanel != panelObj)
                {
                    tempStack.Push(topPanel);
                }
            }
            panelStack.Push(panelObj);
            while (tempStack.Count > 0)
            {
                panelStack.Push(tempStack.Pop());
            }
        }
    }
    
    public void RegisterLuaPanel(string panelName, LuaTable luaTable)
    {
        luaPanels[panelName] = luaTable;
    }
}
