


Class.Define("PresenterLoginServerInfo", {	
	
	Init = function( self, view )
	
		self.view = view
		
	end,
	
	
	Apply = function( self, model )
	
		self.view.Name.text = model.DisplayName
	
	end,
	
	
	Cmd_Select = function( self )
		
	end,
	
	
})