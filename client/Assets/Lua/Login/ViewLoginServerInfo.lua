Class.Define("ViewLoginServerInfo", {


	Awake = function( self )
	
		self.presenter = Class.New("PresenterLoginServerInfo" )
	
		self:Bind( self.presenter )
			
		self.presenter:Init( self )
		
	end,
	
	
	Bind = function( self, presenter  )

		local trans = self.gameObject.transform
		
		-- ²é¿Ø¼þ
		self.Select = trans:Find("Select"):GetComponent("Button")
		self.Name = trans:Find("Select/Name"):GetComponent("Text")
				
	
		-- Ö¸Áî
		self.Select.onClick:AddListener( function( )
			presenter:Cmd_Select( )
		end)
				
		
		
	end,
})

