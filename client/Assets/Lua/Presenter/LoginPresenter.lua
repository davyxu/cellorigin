require "Model/LoginModel"

local PlayerPrefs = UnityEngine.PlayerPrefs

Class.Define("LoginPresenter", {

	PropertySet = Class.New("PropertySet"),

	Init = function( self )
	
		Model.Listen( "LoginModel", "Address", function( v ) 
			self.PropertySet:SetValue("Address", v )
		end)
		
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