require "UIBase"

LoginPanel = UIBase:New()

function LoginPanel:OnOpen()
    print("LoginPanel OnOpen")
    
    -- 获取UI组件引用
    self.loginButton = self:FindComponent("LoginButton", typeof(CS.UnityEngine.UI.Button))
    self.accountInput = self:FindComponent("AccountInput", typeof(CS.UnityEngine.UI.InputField))
    self.passwordInput = self:FindComponent("PasswordInput", typeof(CS.UnityEngine.UI.InputField))
    self.closeButton = self:FindComponent("CloseButton", typeof(CS.UnityEngine.UI.Button))
    
    -- 绑定事件
    self:AddClickListener(self.loginButton, function()
        self:OnLoginClick()
    end)
    
    self:AddClickListener(self.closeButton, function()
        self:OnCloseClick()
    end)
end

function LoginPanel:OnLoginClick()
    local account = self.accountInput.text
    local password = self.passwordInput.text
    
    if account == "" or password == "" then
        print("账号或密码不能为空")
        return
    end
    
    print("登录账号: " .. account)
    
    -- 模拟登录逻辑
    if account == "admin" and password == "123456" then
        print("登录成功!")
        -- 关闭登录面板
        CS.UIManager.Instance:ClosePanel("LoginPanel")
    else
        print("账号或密码错误!")
    end
end

function LoginPanel:OnCloseClick()
    CS.UIManager.Instance:ClosePanel("LoginPanel")
end

function LoginPanel:OnClose()
    print("LoginPanel OnClose")
    -- 清理资源
    self.loginButton = nil
    self.accountInput = nil
    self.passwordInput = nil
    self.closeButton = nil
end

return LoginPanel