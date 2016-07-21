
-- Assets/LuaSupport
require "Cell.Class"
require "Cell.Model"
require "Cell.Network"
require "Cell.Utility"
require "Protobuf.luapb"

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
