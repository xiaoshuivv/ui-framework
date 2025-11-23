print("=== 游戏启动脚本 ===")

function StartGame()
    print("游戏初始化开始...")
    
    -- 加载游戏模块
    require "UIBase"
    require "UIConfig"
    
    local success, errorMsg = pcall(function()
        CS.UIManager.Instance:OpenPanel("LoginPanel")
    end)

    if success then
        print("面板打开成功")
    else
        print("面板打开失败:", errorMsg)
    end

    print("游戏初始化完成...")
end

-- 调用启动函数
StartGame()

return StartGame