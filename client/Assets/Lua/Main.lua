
-- Assets/LuaSupport
require "Cell.Class"
require "Cell.Model"
require "Cell.Network"
require "Cell.Utility"
require "Protobuf.luapb"

-- 常量通过metatable进行限定
LoginConstant = 
{
	DevAddress = "127.0.0.1:8101",
	PublicAddress = "www.test.com:8101",
}


function RequireModule( name )

	require( string.format( "%s.View%s", name, name ) )
	require( string.format( "%s.Model%s", name, name ) )
	require( string.format( "%s.Presenter%s", name, name ) )

end

RequireModule "Login"
require "Login.PresenterLoginServerInfo"
require "Login.ViewLoginServerInfo"

	
--主入口函数。从这里开始lua逻辑
function Main()

	Network.Init( )
	
	Model.Init( )

	Time.timeSinceLevelLoad = 0

end
