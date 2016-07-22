
-- Assets/LuaSupport
require "Protobuf.luapb"
require "Cell.Class"
require "Cell.Model"
require "Cell.Network"
require "Cell.Utility"
require "Cell.Framework"


LoginConstant = 
{
	DevAddress = "127.0.0.1:8101",
	PublicAddress = "www.test.com:8101",
}

	
--主入口函数。从这里开始lua逻辑
function Main()

	Network.Init( )
	
	Model.Init( )
	
	Framework.CreateUI( "Login" )

	Time.timeSinceLevelLoad = 0

end
