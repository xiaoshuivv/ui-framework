UIBase = {}

function UIBase:New(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
end

function UIBase:OnOpen()
    -- 子类重写此方法
    print("UIBase OnOpen: " .. tostring(self.gameObject.name))
end

function UIBase:OnClose()
    -- 子类重写此方法
    print("UIBase OnClose: " .. tostring(self.gameObject.name))
end

function UIBase:OnShow()
    -- 子类重写此方法
end

function UIBase:OnHide()
    -- 子类重写此方法
end

function UIBase:FindComponent(path, componentType)
    local targetObj = self.transform:Find(path).gameObject
    return targetObj:GetComponent(componentType)
end

function UIBase:AddClickListener(button, callback)
    if button and callback then
        button.onClick:AddListener(callback)
    end
end

return UIBase
