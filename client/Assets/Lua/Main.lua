
-- Assets/LuaSupport
require "Cell.Class"
require "Cell.Model"
require "Cell.Network"
require "Cell.Utility"
require "Protobuf.luapb"

function RequireModule( name )

	require( string.format( "%s.View_%s", name, name ) )
	require( string.format( "%s.Model_%s", name, name ) )
	require( string.format( "%s.Presenter_%s", name, name ) )

end

RequireModule "Login"

	
--主入口函数。从这里开始lua逻辑
function Main()

	Network.Init( )

	Time.timeSinceLevelLoad = 0

end
