require "Class"

--主入口函数。从这里开始lua逻辑
function Main()					
	 		local TestClass = Class("TestClass")


function TestClass:foo( a )
	print( self,a )
end


local ins = TestClass.New()
ins:foo("hello")
print(ins)

end

--场景切换通知
function OnLevelWasLoaded(level)
	Time.timeSinceLevelLoad = 0
end