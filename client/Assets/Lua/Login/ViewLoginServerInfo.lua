Class.Define("ViewLoginServerInfo", {


	Awake = function( self )
	
		self.presenter = Class.New("PresenterLoginServerInfo" )
	
		self:Bind( self.presenter )
			
		self.presenter:Init( self )
		
	end,
	
	
	Bind = function( self, presenter  )

		local trans = self.gameObject.transform
				
		self.Select = trans:Find("Select"):GetComponent("Button")
		self.Name = trans:Find("Select/Name"):GetComponent("Text")
				
			
		self.Select.onClick:AddListener( function( )
			presenter:Cmd_Select( )
		end)
				
		
		
	end,
	
	
	
})

