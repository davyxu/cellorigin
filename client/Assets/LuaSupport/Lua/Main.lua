

Class.Define("TestClass", {

	Awake = function( self )

		print( "awake",self)
		
	end,
	
	Start = function( self )

		print( "Start" ,self.gameObject.name)

		
	end,
	--[[
	Update = function( self )

		--print( "Update",self)
		
	end,
	]]
	



})


    
	
	
--主入口函数。从这里开始lua逻辑
function Main()


end

--场景切换通知
function OnLevelWasLoaded(level)
	Time.timeSinceLevelLoad = 0
end