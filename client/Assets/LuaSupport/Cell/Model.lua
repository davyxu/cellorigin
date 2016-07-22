Model = {}

local modelMeta = {}

local function notifyChange( name, key, value, op )
	
	local callbackchain = modelMeta[name]

	
	if callbackchain == nil then
		return
	end
	
	
	for _, callback in ipairs(callbackchain) do	
	
		callback( value, key, op )
		
	end

end

--[[
path格式:
	
callback参数: value, key, op

optional的结构体op始终为"mod"
repeated的结构体op可能为"add" "mod" "del"

]]

function Model.Listen( name, callback )

	
	local callbackchain = modelMeta[name]
	
	if callbackchain == nil then
		callbackchain = {}
		modelMeta[name] = callbackchain
	end
	
	table.insert( callbackchain, callback )
	
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
				
				local op
				
				if finalValue == nil then
					op = "del"
				else
				
					if list[listValue.ModelKey] == nil then
						op = "add"
					else
						op = "mod"
					end
				end
							
				
				list[listValue.ModelKey] = finalValue
				
				notifyChange( k, listValue.ModelKey, finalValue, op )

			end
		
		
		else
		-- 单个
			ModelDataRoot[k] = v
			notifyChange( k, nil, v, "mod" )
		
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

	LoginPeer:RegisterMessage("gamedef.ModelACK", function( msg )
			
		Model.Apply( msg )

	end )
	
	
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


