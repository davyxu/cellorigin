
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


function Class.New( className, ... )

	local descriptor = getClassDescriptor( className )
	
	local ins = setmetatable( {}, descriptor )
	
	if ins.New ~= nil then
		ins:New(...)
	end
		
	return ins

end

-- Unity在脚本中存在时会调用这些函数
local unityMethodList = {
    "Awake",
    "OnEnable",
    "Start",
    "OnDisable",
    "OnDestory",
}

function Class.HasMethod( className )

    local descriptor = getClassDescriptor( className )
    
    local ret = {}
    
    for _, name in ipairs(unityMethodList) do
    
        local has = type(descriptor[name]) == "function"
    
        table.insert( ret, has )
    
    end

    return ret
end
   

local instanceMap = {}

function Class.CallMethod( className, name, go )

	local descriptor = getClassDescriptor( className )
	
	
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