Class.Define("LoginView", {


	Awake = function( self )
	
		self.presenter = Class.New("LoginPresenter" )
	
		self:Bind( self.presenter )
			
		self.presenter:Init( self )
		
		LuaPB.RegisterFile("game.pb")
		
		-- local data = LuaPB.GetTestData()
		
		local size = luapb_bytesize( "tutorial.Person", {
			name = "hello",
			--test = {1, 2},	
		})
		
		print(size)
		
		--[[
		
		LoginPeer:Connect( "127.0.0.1:8101" )
		
		LoginPeer:RegisterMessage("gamedef.PeerConnected", function( )
		
			print("connected")
		
			SendLoginMessage( "gamedef.LoginREQ", {
				PlatformName = "dev",
			})
		
		end )
		
		LoginPeer:RegisterMessage("gamedef.LoginACK", function( msg )
		
			print("loginACK",msg.Result)
		
		end )
		
		
		]]
	end,
	
	
	Bind = function( self, presenter  )

		local trans = self.gameObject.transform
		
		-- 查控件
		self.Account = trans:Find("Account"):GetComponent("InputField")
		self.Address = trans:Find("Address"):GetComponent("InputField")
		
		self.SetDevAddress = trans:Find("SetDevAddress"):GetComponent("Button")
		self.SetPublicAddress = trans:Find("SetPublicAddress"):GetComponent("Button")
		
		
	-- 指令
		self.SetDevAddress.onClick:AddListener( function( )
			presenter:Cmd_SetDevAddress( )
		end)
		
		self.SetPublicAddress.onClick:AddListener( function( )
			presenter:Cmd_SetPublicAddress( )
		end)
		
		
	end,
	
	
	OnDisable = function( self )		
	
		self.presenter:SaveSetting()
	
	end,

})

