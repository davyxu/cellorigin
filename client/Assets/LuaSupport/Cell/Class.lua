
local classDescriptor = {}



function DefineClass( name )	

	if classDescriptor[name] then
		error("duplicate class name: " .. name)
	end
	
	local newClass = {}
	
	classDescriptor[name] = newClass
	
	newClass.__index = newClass
	
	return newClass

end


function NewClass( name )

	local desc = classDescriptor[name]

	if desc == nil then
		error("class not defined: " .. name)
	end
	
	local ins = setmetatable( {}, desc )
		
	return ins

end
