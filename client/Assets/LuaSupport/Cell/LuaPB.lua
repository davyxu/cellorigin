local FieldType_Message = 11

local fieldSizeMapper = {

	-- float
	[2] = 4,

	-- int32
	[5] = LuaPB.Int32Size,

	-- bool
	[8] = 1,

	-- string
	[9] = function( str )
		local len = #str

		return LuaPB.VarintSize32( len ) + len
	end,
}


local function FieldSize( fd, value )

	local v = fieldSizeMapper[fd.Type]
	local typeOfV = type(v)
	if typeOfV == "function" then
		return v(value)
	end

	return v
end


local function RawByteSize( msgD, t )

	local size = 0

	for k, v in pairs( t ) do
	
		local fd = msgD:GetFieldByName( k )
		
		if fd ~= nil then
		
			if fd.Type == FieldType_Message then
			
				if fd.IsRepeated then
			
					for listK, listV in ipairs(v) do
					
						size = size + LuaPB.TagSize( fd.Number )
						
						local structSize = RawByteSize( fd.MessageType, listV )
						size = size + LuaPB.VarintSize32( structSize ) + structSize
					
					end
					
				else
				
					size = size + LuaPB.TagSize( fd.Number )
					size = size + RawByteSize( fd.MessageType, v )
				end
			
			else 
				-- 普通值
				
				if fd.IsRepeated then
				
					for listK, listV in ipairs(v) do
						size = size + LuaPB.TagSize( fd.Number )
						size = size + FieldSize( fd, listV )
					end
					
				else
				
					size = size + LuaPB.TagSize( fd.Number )
					size = size + FieldSize( fd, v )
				
				end
			
			end 
		
		
		end -- if fd ~= nil then
		
	end
	
	return size


end


local function RawEncode( msgD, t )


	for k, v in pairs( t ) do
	
		local fd = msgD:GetFieldByName( k )
		
		if fd ~= nil then
		
			if fd.Type == FieldType_Message then
			
				if fd.IsRepeated then
			
					for listK, listV in ipairs(v) do
					
					
					end
					
				else
				
				end
			
			else 
				-- 普通值
				
				if fd.IsRepeated then
				
					for listK, listV in ipairs(v) do

					end
					
				else
				
					
				
				end
			
			end 
		
		
		end -- if fd ~= nil then
		
	end

end



function luapb_bytesize( name,  t )

	local pool = LuaPB.GetPool()
	
	local msgD = pool:GetMessage( name )

	return RawByteSize( msgD, t )
end


function luapb_encode( name, t )

	local pool = LuaPB.GetPool()
	
	local msgD = pool:GetMessage( name )

	
end

