
local PlayerPrefs = UnityEngine.PlayerPrefs


Class.Define("LoginPresenter", {	
	
	Init = function( self, view )
	
		BindData( "LoginModel", "Address", view )
	
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
	
		LoginModel.Account = PlayerPrefs.GetString("Login.Account")
		LoginModel.Address = PlayerPrefs.GetString("Login.Address")
	end,
	
	SaveSetting = function( self )
	
		PlayerPrefs.SetString("Login.Account", LoginModel.Account )
		PlayerPrefs.SetString("Login.Address", LoginModel.Address )

	end,
	
})