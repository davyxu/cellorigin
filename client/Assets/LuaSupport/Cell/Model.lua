Model = {}

local modelMap = {}

local function getDefine( name )

	local m = modelMap[name]
	
	if m == nil then
		error("can not found model: ".. name)
	end

	return m

end


local function notifyChange( name, key, value )

	--print("changed", name, key, value)

	local m = getDefine(name)
	
	local callbackchain = m.listener[key]

	
	if callbackchain == nil then
		return
	end
	
	
	for _, callback in ipairs(callbackchain) do	
	
		callback( value )
		
	end

end

function Model.Define( name, descriptor )

	if _G[name] then
		error("model name exist in _G: " .. name )
	end


	local instance =  setmetatable( {}, {
		__newindex = function( self, key, value)
			local v = rawget( descriptor, key ) 
			if v == nil then
				error("can not dynamic create model value: ".. name .. "." .. key)
			else
				rawset( descriptor, key, value )
				notifyChange( name, key, value )
			end
		end,
		
		__index = descriptor,
		
	})
	
	
	modelMap[name] = {
		instance = instance,
		descriptor = descriptor,
		listener = {},
	}
	
	_G[name] = instance	
end




function Model.Listen( name, key, callback )

	local m = getDefine(name)
	
	local callbackchain = m.listener[key]
	
	if callbackchain == nil then
		callbackchain = {}
	end
	
	table.insert( callbackchain, callback )
	
	m.listener[key] = callbackchain

end

--[[
local m = Model.Define( "a", {

	foo = 1,

})

Model.Listen("a", "foo", function(t, k )
	print("changed", t, k )
end)


m.foo= 2
]]


function BindData( modelName, modelKey, view, viewPropertyName, filterFunc )

	viewPropertyName = viewPropertyName and viewPropertyName or modelKey

	Model.Listen( modelName, modelKey, function( v ) 
		
		local obj = view[viewPropertyName]
		
		if type(obj) == "userdata" then
		
			if filterFunc == nil then
				obj.text = tostring(v)
			else
				obj.text = filterFunc(v)
			end
			
		end

	end)

end


local ModelRoot = {}

local function ApplyModel( tModel, msg )

	for k, v in pairs(msg) do
	
		if type(v) == "table" then
		
			-- 这是个repeated
			if #v > 0 then
			
				local map = tModel[k]
			
				if map == nil then
					 map = {}
					 tModel[k] = map
				end
			
			
				for i, av in ipairs(v) do
				
					if av.ModelDelete then
					
						if av.ModelKey == nil then
						
							map[i] = nil
						
						else
							
							map[av.ModelKey] = nil
						
						end
					
					else
					
					
						if av.ModelKey == nil then
						
							map[i] = av
						
						else
							
							map[av.ModelKey] = av
						
						end
						
					
					end
				


				end
			
			
			else
			-- 这是个optional
			
				tModel[k] = v
			
			end
		
		
		else
		
			tModel[k] = v
		
		end
	
	end


end


function Model.Init( )

	LoginPeer:RegisterMessage("gamedef.ModelACK", function( msg )
			
		ApplyModel( ModelRoot, msg )

	end )

end

