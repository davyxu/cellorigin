Model = {}

local modelMeta = {}

-- ModelACK的描述
local modelRootDescriptor

local function notifyChange( modelName, modelKey, modelValue, op )
	
	local callbackchain = modelMeta[modelName]

	
	if callbackchain == nil then
		return
	end
	
	
	for _, callback in ipairs(callbackchain) do	
	
		callback( modelValue, modelKey, op )
		
	end

end

--[[
侦听model变化

path格式:
	
callback参数: modelValue, modelKey, op

optional的结构体op始终为"mod"
repeated的结构体op可能为"add" "mod" "del"

]]

function Model.Listen( modelName, callback )

	
	local callbackchain = modelMeta[modelName]
	
	if callbackchain == nil then
		callbackchain = {}
		modelMeta[modelName] = callbackchain
	end
	
	table.insert( callbackchain, callback )
	
end



local ModelDataRoot = {}


local function ModValue( modelName, modelValue, doNotify )

	local modelD = modelRootDescriptor:GetFieldByName( modelName )
		
	if modelD == nil then
		error("field not found:" .. modelName )
	end
	
	-- 多个
	if modelD.IsRepeated then
	
		for listIndex, listValue in ipairs( modelValue ) do
		
		
			if listValue.ModelID == nil then
				error("repeated model not set 'ModelID' , " ..modelName )
			end
			
			local list = ModelDataRoot[modelName]
		
			if list == nil then
				 list = {}
				 ModelDataRoot[modelName] = list
			end
			
			local finalValue
		
			if not listValue.ModelDelete then
				finalValue = listValue
			end
			
			local op
			
			if finalValue == nil then
				op = "del"
			else
			
				if list[listValue.ModelID] == nil then
					op = "add"
				else
					op = "mod"
				end
			end
						
			
			list[listValue.ModelID] = finalValue
			
			if doNotify ~= false then
				notifyChange( modelName, listValue.ModelID, finalValue, op )
			end 

		end
	
	
	else
	-- 单个
		ModelDataRoot[modelName] = modelValue
		
		if doNotify ~= false then
			notifyChange( modelName, nil, modelValue, "mod" )
		end
	
	end
end


-- 将msg的内容完整覆盖到对应的model
function Model.Apply( msg )	
	
	for modelName, modelValue in pairs(msg) do
	
		ModValue( modelName, modelValue )

	end


end

-- 获取model数据
function Model.Get( modelName, modelKey )

	local msg = ModelDataRoot[modelName]
	if msg == nil then
		return nil
	end
	
	if modelKey == nil then
		return msg
	end

	return msg[modelKey]

end

-- 修改model数据， 回调返回model结构
function Model.Modify( modelName, callback, doNotify )	

	local model = ModelDataRoot[modelName]
	
	model = model or {}
	
	callback(model )
	
	ModValue( modelName, model, doNotify )
	
end


function Model.Init( )

	modelRootDescriptor = LuaPB.GetPool():GetMessage( "gamedef.ModelACK" )

	LoginPeer:RegisterMessage("gamedef.ModelACK", function( msg )
			
		Model.Apply( msg )

	end )

	
end
