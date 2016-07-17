
-- Assets/LuaSupport
require "Cell.Class"
require "Cell.Model"
require "Cell.Network"
require "Cell.Utility"
require "Protobuf.luapb"

-- Assets/Lua
require "Presenter.LoginPresenter"
require "View.LoginView"
require "Model.LoginModel"    

	
--主入口函数。从这里开始lua逻辑
function Main()

	Network.Init( )

	Time.timeSinceLevelLoad = 0

end
