Class.Define("LoginView", {


	Awake = function( self )
	
		self.presenter = Class.New("LoginPresenter")
	
		self:Bind( self.presenter )
	
	end,
	
	
	Bind = function( self, presenter )

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
		
		-- 属性
		presenter.PropertySet:Register("Account", self.Account)
		presenter.PropertySet:Register("Address", self.Address)
		
		
		
	end,
	
	OnDisable = function( self )
		print("ondisable")
	
		self.presenter:SaveSetting()
	
	end,
	
	



})

