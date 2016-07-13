local FieldType_Float = 2
local FieldType_Int32 = 5
local FieldType_Bool = 8
local FieldType_String = 9
local FieldType_Message = 11
local FieldType_Enum = 14

local fieldSizeMapper = {

	-- float
	[FieldType_Float] = 4,

	-- int32
	[FieldType_Int32] = LuaPB.Int32Size,

	-- bool
	[FieldType_Bool] = 1,

	-- string
	[FieldType_String] = function( str )
		local len = #str

		return LuaPB.VarintSize32( len ) + len
	end,
}

local fieldWriteMapper = {

	-- float
	[FieldType_Float] = PBStream.WriteFloat32,

	-- int32
	[FieldType_Int32] = PBStream.WriteInt32,

	-- bool
	[FieldType_Bool] = PBStream.WriteBool,

	-- string
	[FieldType_String] = PBStream.WriteString,

}

local function WriteValue( stream, fd, value )

	if fd.Type == FieldType_Enum then
	
		local et = fd.EnumType
		
		if et ~= nil then
		
			local evd = et:GetValueByName( value )
	
			stream:WriteInt32( fd.Number, evd.Number )
		
		end
	
	else

		local v = fieldWriteMapper[fd.Type]
		local typeOfV = type(v)

		if typeOfV == "function" then
			return v( stream, fd.Number, value)
		else
			error("unknown field type:" .. fd.Type )
		end
	
	end

	
end


local function FieldSize( fd, value )

	if fd.Type == FieldType_Enum then
	
		local et = fd.EnumType
		
		if et ~= nil then
		
			local evd = et:GetValueByName( value )
	
			return LuaPB.Int32Size( evd.Number )
		
		end
		
		return 0
		
	else
	

		local v = fieldSizeMapper[fd.Type]
		local typeOfV = type(v)
		if typeOfV == "function" then
			return v(value)
		else
			error("unknown field type:" .. fd.Type )
		end
		

		return v
	
	end
	
	
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


local function RawEncode( stream, msgD, t )


	for k, v in pairs( t ) do
		
		local fd = msgD:GetFieldByName( k )
		
		if fd ~= nil then
		
			if fd.Type == FieldType_Message then
			
				if fd.IsRepeated then
			
					for listK, listV in ipairs(v) do
					
						stream:WriteMessageHeader( fd.Number, RawByteSize( fd.MessageType, listV ) )
						RawEncode( stream, fd.MessageType, listV )
						
					end
					
				else
				
					stream:WriteMessageHeader( fd.Number, RawByteSize( fd.MessageType, v ) )
					RawEncode( stream, fd.MessageType, v )
				
				end
			
			else 
				-- 普通值
				
				if fd.IsRepeated then
				
					for listK, listV in ipairs(v) do
						WriteValue( stream, fd, listV ) 
					end
					
				else
				
					WriteValue( stream, fd, v ) 
				
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

	local stream = PBStream.New()
	
	RawEncode( stream, msgD, t )
	
	return stream
	
end

