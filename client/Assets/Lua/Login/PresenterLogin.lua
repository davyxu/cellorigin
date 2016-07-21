
local PlayerPrefs = UnityEngine.PlayerPrefs
local Resources  = UnityEngine.Resources
local GameObject = UnityEngine.GameObject 

Class.Define("PresenterLogin", {	
	
	Init = function( self, view )

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
		 
		
		self:LoadSetting()
		
		local listWidget = view.LoginServerList
		local content = listWidget.transform:Find("Viewport/Content")
		
		self.listData = {}
		
		Model.Listen( "ServerList", function( value, key, op )
			print( value, key, op )
		
			if op == "add" then
			
				local prefab = Resources.Load("View/LoginServerInfo")
						
				local newItem = GameObject.Instantiate(prefab)
				newItem.transform:SetParent( content.transform, false )
				local newItemClass = newItem.gameObject:AddComponent(typeof(CellLuaClass))
				newItemClass:InitAwake("ViewLoginServerInfo")
				local instance = Class.GetInstance( newItemClass.gameObject )
				instance.presenter:Apply( value )
				
				self.listData[key] = instance.presenter
			
			elseif op == "mod" then
				
				local presenter = self.listData[key]
				presenter:Apply( value )
		
			elseif op == "del" then
				
				local presenter = self.listData[key]
				GameObject.Destroy( presenter.view.gameObject )
				self.listData[key] = nil
			end
		
		end )
		
		Model.Apply
		{
			ServerList = 
			{
				{ ModelKey = "100", DisplayName = "hello" },
			},
		}
		
		Model.Apply
		{
			ServerList = 
			{
				{ ModelKey = "100", DisplayName = "hello2" },
			},
		}
		
		Model.Apply
		{
			ServerList = 
			{
				{ ModelKey = "100", ModelDelete = true },
			},
		}
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