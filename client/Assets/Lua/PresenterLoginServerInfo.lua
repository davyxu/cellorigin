Class.Define("PresenterLoginServerInfo", {	
	
	Awake = function( self )
	
		require("ViewLoginServerInfo")( self )		
	end,
	
	
	ApplyModel = function( self, model )
	
		Framework.SetViewText( self, "Name", string.format("%s|%s",model.DisplayName, model.Address) )
	end,
	
	
	Command = {
	
		Select = function( self )
			print("select ".. self.UI.Name.text )
		end,
	
	},
	
	
})