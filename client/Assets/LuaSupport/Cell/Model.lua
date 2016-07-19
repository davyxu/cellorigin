Model = {}

local modelMeta = {}

local function notifyChange( model, key, value )
	
	local path
	if key == nil then
		path = model
	else
		path = string.format("%s.%s", model, key )
	end
	
	
	local callbackchain = modelMeta[path]

	
	if callbackchain == nil then
		return
	end
	
	
	for _, callback in ipairs(callbackchain) do	
	
		callback( value )
		
	end

end

-- model.field

-- model.key.field


function Model.Listen( path, callback )

	
	local callbackchain = modelMeta[path]
	
	if callbackchain == nil then
		callbackchain = {}
		modelMeta[path] = callbackchain
	end
	
	table.insert( callbackchain, callback )
	
end



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


function Model.Bind( view, viewPropertyName )


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




local ModelDataRoot = {}
function Model.Apply( msg )	
	
	local rootD = LuaPB.GetPool():GetMessage( "gamedef.ModelACK" )
	
	for k, v in pairs(msg) do
	
		local modelD = rootD:GetFieldByName( k )
		
		if modelD == nil then
			error("field not found:" .. k )
		end
		
		-- 多个
		if modelD.IsRepeated then
		
			for listIndex, listValue in ipairs( v ) do
			
				if listValue.ModelKey == nil then
					error("repeated model not set 'ModelKey' , " ..k )
				end
			
				local list = ModelDataRoot[k]
			
				if list == nil then
					 list = {}
					 ModelDataRoot[k] = list
				end
				
				local finalValue
			
				if not listValue.ModelDelete then
					finalValue = listValue
				end
				
				list[listValue.ModelKey] = finalValue
				
				notifyChange( k, listValue.ModelKey, finalValue )

			end
		
		
		else
		-- 单个
		
			local finalValue
			
			if not v.ModelDelete then
				finalValue = v
			end
			
			ModelDataRoot[k] = finalValue
			notifyChange( k, nil, finalValue )
		
		end
	

	end


end


function Model.Get( model, key )

	local msg = ModelDataRoot[model]
	if msg == nil then
		return nil
	end
	
	if key == nil then
		return msg
	end

	return msg[key]

end

function Model.Init( )



--[[
	LoginPeer:RegisterMessage("gamedef.ModelACK", function( msg )
			
		ApplyModel( ModelRoot, msg )

	end )
	
	]]

end


local function UnitTest( )

	-- 添加
	ApplyModel{
	
		Account = {
			Name = "hello",
		},
	
		Role = { 
		
			{
				ModelKey = "1",
				HP = 1,
			},
		
		},
	
	
	}
	
	dump( ModelRoot )
	
	
	-- 修改
	ApplyModel{
	
		Account = {
			Name = "hello2",
		},
	
		Role = { 
		
			{
				ModelKey = "1",
				HP = 2,
			},
			
			{
				ModelKey = "2",
				HP = 100,
			},
		
		},
	
	
	}
	
	dump( ModelRoot )
	
	-- 删除
	ApplyModel{
	
		Account = {
			Name = "hello2",
		},
	
		Role = { 
		
			{
				ModelKey = "1",
				ModelDelete = true,				
			},
						
		
		},
	
	
	}
	
	dump( ModelRoot )

end


