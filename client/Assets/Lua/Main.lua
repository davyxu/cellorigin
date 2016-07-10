require "Class"
require "Model"
require "Network"
require "Utility"

require "Presenter/LoginPresenter"
require "View/LoginView"
require "Model/LoginModel"    
	
	
--主入口函数。从这里开始lua逻辑
function Main()
	
	Network.Init( "Assets/game.pb" )

	Time.timeSinceLevelLoad = 0

end
