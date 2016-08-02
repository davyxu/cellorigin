
local PlayerPrefs = UnityEngine.PlayerPrefs


local function LoadSetting( self )

	Model.Modify( "Login", function( v ) 

			v.Account = PlayerPrefs.GetString("Login.Account")
			v.Address = PlayerPrefs.GetString("Login.Address")
	end )
	
	
end

local function SaveSetting( self )

	local loginModel = Model.Get( "Login" )	

	PlayerPrefs.SetString("Login.Account", loginModel.Account )
	PlayerPrefs.SetString("Login.Address", loginModel.Address )

end
	
	
Class.Define("Login", {	

	Awake = function( self )
	
		require("View.Login")( self )
	
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

		-- view到model的侦听在手游里很少， 所以特殊处理下就好
		Framework.ViewListen( self, "Account", function( v )
			Model.Modify( "Login", function( m )
				m.Account = v
			end, false )
			
		end )
		
		Framework.ViewListen( self, "Address", function( v )
			Model.Modify( "Login", function( m )
				m.Address = v
			end, false )
			
		end )



	end,

	
	Command =
	{
	
		SetDevAddress = function( self )
		
			Model.Modify("Login", function( v )
			
				v.Address = LoginConstant.DevAddress
			
			end )

			
		end,
		
		
		SetPublicAddress = function( self )
		
			Model.Modify("Login", function( v )
			
				v.Address = LoginConstant.PublicAddress
			
			end )
			
		end,
		
	},
	

	
	Start = function( self )
	
		LoginPeer:Connect( "127.0.0.1:8101" )
	
	end,
	
	
	OnDisable = function( self )		
	
		SaveSetting()
	
	end,
	
})