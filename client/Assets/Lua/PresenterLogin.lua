
local PlayerPrefs = UnityEngine.PlayerPrefs


local function LoadSetting( self )

	Model.Apply
	{
		Login = 
		{
			Account = PlayerPrefs.GetString("Login.Account"),
			Address = PlayerPrefs.GetString("Login.Address"),
		}
	}
	
end

local function SaveSetting( self )

	local loginModel = Model.Get( "Login" )

	PlayerPrefs.SetString("Login.Account", loginModel.Account )
	PlayerPrefs.SetString("Login.Address", loginModel.Address )

end
	
	
Class.Define("PresenterLogin", {	

	Awake = function( self )
	
		require("ViewLogin")( self )
	
		-- 散界面的手动绑定法
		Model.Listen( "Login", function( v )
		
			Framework.SetViewText( self, "Address", v.Address )
			Framework.SetViewText( self, "Account", v.Account )
		
		end )
		
		-- 将界面绑定到model, 数据增删改自动更新列表
		Framework.BindModelToList( self, "ServerList", "LoginServerInfo")
		
		LoginPeer:RegisterMessage("gamedef.PeerConnected", function( )
		
			SendLoginMessage( "gamedef.LoginREQ", {
				PlatformName = "dev",
			})
		
		end )
		
		
		LoginPeer:RegisterMessage("gamedef.LoginACK", function( msg )
		
			print("loginACK", msg.Result)
		
		end )
		
		LoadSetting()

	end,

	
	Command =
	{
	
		SetDevAddress = function( self )
			LoginModel.Address = LoginConstant.DevAddress
		end,
		
		
		SetPublicAddress = function( self )
			LoginModel.Address = LoginConstant.PublicAddress
		end,
		
	},
	

	
	Start = function( self )
	
		LoginPeer:Connect( "127.0.0.1:8101" )
	
	end,
	
	
	OnDisable = function( self )		
	
		SaveSetting()
	
	end,
	
})