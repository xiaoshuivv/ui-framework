# ui-framework
Unity XLua C# ui-framework

**框架核心功能特点：**
- **完整的生命周期管理**：包含OnOpen、OnShow、OnClose、OnHide等完整的界面生命周期方法
- **UI组件绑定**：通过AddClickListener方法绑定Button组件的点击事件
- **配置化UI管理**：通过UIConfig.lua统一管理所有UI界面的路径和配置
- **栈式UI管理**：支持返回操作和界面堆栈管理

**技术实现机制：**
- C#端负责资源管理和实例缓存，通过XLua与Lua脚本交互
- Lua端实现具体的业务逻辑，提供面向对象的继承机制
- 支持热更新，所有业务逻辑都在Lua端实现

**使用流程说明：**
1. 在Unity中创建GameManager GameObject并挂载脚本
2. 将Lua脚本放置于LuaScripts目录下
3. 创建UI预制体并放入Resources/UI目录
4. 在UIConfig.lua中配置UI路径和层级
5. 在Lua中继承UIBase创建具体面板
