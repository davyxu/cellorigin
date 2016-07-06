
local descriptorMap = {}

Class = {}


local function getClassDescriptor( className )
	local descriptor = descriptorMap[className]

	if descriptor == nil then
		error("class not defined: " .. className)
	end
	
	return descriptor
	
end

function Class.Define( className, descriptor )	

	if descriptorMap[className] then
		error("duplicate class className: " .. className)
	end
	descriptor.__index = descriptor
	descriptorMap[className] = descriptor

end


function Class.New( className )

	local descriptor = getClassDescriptor( className )
	
	local ins = setmetatable( {}, descriptor )
	
	if ins.Init ~= nil then
		ins:Init()
	end
		
	return ins

end



local instanceMap = {}

function Class.CallMethod( className, name, go )

	local descriptor = getClassDescriptor( className )
	
	
	local ins
	
	if name == "Awake" then
	
		ins = Class.New(className)
		
		ins.gameObject = go
		
		instanceMap[go] = ins
		
	else
	
		ins = instanceMap[go]
		
		if ins == nil then
			error("instance not found", gameObject.name )
		end
		
	
	
	end
	
	
	local method = descriptor[name]
	if method then
		method( ins )
	end
	
	if name == "OnDestory" then
	
		instanceMap[go] = nil
	
	end
end