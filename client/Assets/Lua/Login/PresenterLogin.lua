
local PlayerPrefs = UnityEngine.PlayerPrefs


Class.Define("PresenterLogin", {	
	
	Init = function( self, view )
	
		self:LoadSetting()
		
		Model.Listen( "Login", function( v )
		
			view.Address.text = v.Address
			view.Account.text = v.Account
		
		end )
		
		LoginPeer:RegisterMessage("gamedef.PeerConnected", function( )
		
			SendLoginMessage( "gamedef.LoginREQ", {
				PlatformName = "dev",
			})
		
		end )
		
		
		LoginPeer:RegisterMessage("gamedef.LoginACK", function( msg )
		
			print("loginACK", msg.Result)
		
		end )
		 
		
	end,
	
	Cmd_SetDevAddress = function( self )
		LoginModel.Address = LoginConstant.DevAddress
	end,
	
	
	Cmd_SetPublicAddress = function( self )
		LoginModel.Address = LoginConstant.PublicAddress
	end,
	
	Cmd_Start = function( self )
	
		LoginPeer:Connect( "127.0.0.1:8101" )
		
	end,
	
	
	LoadSetting = function( self )
	
		
		Model.Apply
		{
			Login = 
			{
				Account = PlayerPrefs.GetString("Login.Account"),
				Address = PlayerPrefs.GetString("Login.Address"),
			},
		}
		
	end,
	
	SaveSetting = function( self )
	
		local loginModel = Model.Get( "Login" )
	
		PlayerPrefs.SetString("Login.Account", loginModel.Account )
		PlayerPrefs.SetString("Login.Address", loginModel.Address )

	end,
	
})