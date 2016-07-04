
Class.Define("PropertySet", {

	propMap = {},

	Register = function( self, name, obj )
	
		self.propMap[name] = obj
	
	end,
	
	GetValue = function( self, name )
				
		local p = self.propMap[name]
		
		if type(p) == "userdata" then
			return p.text
		end
		
		return ""

	end,
	
	SetValue = function( self, name, v )
	
		local p = self.propMap[name]
		
		if type(p) == "userdata" then
			p.text = tostring(v)
		end

	end,
	
	
	
})
