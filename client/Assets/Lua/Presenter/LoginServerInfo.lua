Class.Define("LoginServerInfo", {	
	
	Awake = function( self )
	
		require("View.LoginServerInfo")( self )		
	end,
	
	
	ApplyModel = function( self, model )
	
		self.model = model
		
		Framework.SetViewText( self, "Name", string.format("%s|%s",model.DisplayName, model.Address) )
	end,
	
	
	Command = 
	{
	
		Select = function( self )
			print("select ".. self.model.DisplayName )
			
			GamePeer:Connect( model.Address )
		end,
	
	},
	
	
})