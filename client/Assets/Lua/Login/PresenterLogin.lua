
local PlayerPrefs = UnityEngine.PlayerPrefs


Class.Define("PresenterLogin", {	
	
	Init = function( self, view )
	
		BindData( "ModelLogin", "Address", view )
	
		self:LoadSetting()
		
	end,
	
	Cmd_SetDevAddress = function( self )
		LoginModel.Address = LoginConstant.DevAddress
	end,
	
	
	Cmd_SetPublicAddress = function( self )
		LoginModel.Address = LoginConstant.PublicAddress
	end,
	
	Cmd_Start = function( self )
		self:StartLogin( )
	end,
	
	StartLogin = function( self )
	
	end,
	
	
	LoadSetting = function( self )
	
		ModelLogin.Account = PlayerPrefs.GetString("Login.Account")
		ModelLogin.Address = PlayerPrefs.GetString("Login.Address")
	end,
	
	SaveSetting = function( self )
	
		PlayerPrefs.SetString("Login.Account", ModelLogin.Account )
		PlayerPrefs.SetString("Login.Address", ModelLogin.Address )

	end,
	
})