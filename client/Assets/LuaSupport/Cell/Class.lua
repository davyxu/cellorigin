
local descriptorByName = {}
local instanceByGo = {}

Class = {}


local function getClassDescriptor( className )
	local descriptor = descriptorByName[className]

	if descriptor == nil then
		error("class not defined: " .. className)
	end
	
	return descriptor
	
end

function Class.Define( className, descriptor )	

	if descriptorByName[className] then
		error("duplicate class className: " .. className)
	end
	descriptor.__index = descriptor
	descriptorByName[className] = descriptor

end


function Class.New( className, ... )

	local descriptor = getClassDescriptor( className )
	
	local ins = setmetatable( {}, descriptor )
	
	if ins.New ~= nil then
		ins:New(...)
	end
		
	return ins

end

function Class.NewGo( className, go, ...  )

	local ins = Class.New( className, ... )

	instanceByGo[go] = ins
	ins.gameObject = go
	
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
   



function Class.CallMethod( className, name, go )

	local ins = instanceByGo[go]
	
	if ins == nil then
		error("instance not found: " .. className )
	end
	
	local descriptor = getClassDescriptor( className )	
	
	local method = descriptor[name]
	if method then
		method( ins )
	end
	
	if name == "OnDestory" then
	
		instanceByGo[go] = nil
	
	end
end



function Class.GetInstance( go )
	return instanceByGo[go]
end