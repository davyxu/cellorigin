local LuaPB_TagSize = LuaPB.TagSize
local LuaPB_VarintSize32 = LuaPB.VarintSize32

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

		return LuaPB_VarintSize32( len ) + len
	end,
}

local fieldWriteMapper = {

	-- float
	[FieldType_Float] = PBStreamWriter.WriteFloat32,

	-- int32
	[FieldType_Int32] = PBStreamWriter.WriteInt32,

	-- bool
	[FieldType_Bool] = PBStreamWriter.WriteBool,

	-- string
	[FieldType_String] = PBStreamWriter.WriteString,

}

local fieldReaderMapper = {

	-- float
	[FieldType_Float] = PBStreamReader.ReadFloat,

	-- int32
	[FieldType_Int32] = PBStreamReader.ReadInt32,

	-- bool
	[FieldType_Bool] = PBStreamReader.ReadBool,

	-- string
	[FieldType_String] = PBStreamReader.ReadString,

}

local function ReadValue( stream, fd )

	if fd.Type == FieldType_Enum then
	
		local et = fd.EnumType
		
		if et ~= nil then
		
			local ok, number = stream:ReadInt32( )
			
			if ok then		
				
				local evd = et:GetValueByNumber( number )
				
				if evd == nil then
					return ""
				end
		
				return evd.Name
				
			else
				return 0
			
			end
		
		
		end
	
	else
	
		local v = fieldReaderMapper[fd.Type]
	
		local typeOfV = type(v)

		if typeOfV == "function" then
					
			local ok, value = v( stream  )
			
			if ok then
				return value
			else
				error("error read value: ".. fd.Name)
			end
			
		else
			error("unknown field type:" .. fd.Type )
		end
		
	end

end

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
					
						size = size + LuaPB_TagSize( fd.Number )
						
						local structSize = RawByteSize( fd.MessageType, listV )
						size = size + LuaPB_VarintSize32( structSize ) + structSize
					
					end
					
				else
				
					size = size + LuaPB_TagSize( fd.Number )
					size = size + RawByteSize( fd.MessageType, v )
				end
			
			else 
				-- 普通值
				
				if fd.IsRepeated then
				
					for listK, listV in ipairs(v) do
						size = size + LuaPB_TagSize( fd.Number )
						size = size + FieldSize( fd, listV )
					end
					
				else
				
					size = size + LuaPB_TagSize( fd.Number )
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


local function RawDecode( msgD, stream )

	local tab = {}
	
	while true do
	
		local fd = stream:ReadField( msgD )
		
		if fd == nil then
			break
		end
	

		if fd.Type == FieldType_Message then
		
			local ok, limit = stream:BeginMessage( )
			
			local value = RawDecode( fd.MessageType, stream )
		
			stream:EndMessage( limit )
		
			if fd.IsRepeated then
			
				local list = tab[fd.Name]
				
				if type(list) == "table" then
				
					table.insert(list, value )
				
					tab[fd.Name] = list
				
				else
					tab[fd.Name] = {value}
				end
				
			
			else
				tab[fd.Name] = value
			
			end
		
		
		else
		
			local value = ReadValue( stream, fd )
		
			if fd.IsRepeated then
			
				local list = tab[fd.Name]
				
				if type(list) == "table" then
				
					table.insert(list, value )
				
					tab[fd.Name] = list
				
				else
					tab[fd.Name] = {value}
				end
			else
			
				tab[fd.Name] = value
			end
		
		end
	
	end
	
	return tab

end

function luapb_bytesize( name,  t )

	local pool = LuaPB.GetPool()
	
	local msgD = pool:GetMessage( name )

	return RawByteSize( msgD, t )
end


function luapb_encode( name, t )

	local pool = LuaPB.GetPool()
	
	local msgD = pool:GetMessage( name )

	local stream = PBStreamWriter.New()
	
	RawEncode( stream, msgD, t )
	
	stream:Flush()
	
	return stream
	
end

function luapb_decode( name, str )

	local pool = LuaPB.GetPool()
	
	local msgD = pool:GetMessage( name )
	
	local stream = PBStreamReader.New( str )
	
	return RawDecode( msgD, stream )

end

