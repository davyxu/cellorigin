Network ={}

require "pbc/protobuf"

function Network.Init( pbfile )

	--protobuf.register_file(pbfile)
	
	LoginPeer = PeerManager.Instance:Get( "login" )
	

end


function Network.DecodeRecv( peer, msgName, stream, callback )

	if stream == nil then

		print(string.format("[%s] #recv %s", peer.Name, msgName ) )
		
		callback( )
	
	else
	
		local msg, err = protobuf.decode( msgName, stream )
	
		if msg == false then
			logError(msgName .. err)
			return
		end
		
		dump( msg )
		
		print(string.format("[%s] #recv %s|%s", peer.Name, msgName, SeralizeTable( msg ) ) )
		
		callback( msg )
		
	end

end

local function EncodeSend( peer, msgName, msgTable )

	 local code = protobuf.encode(msgName, msgTable)
	 
	 print(string.format("[%s] #send %s|%s", peer.Name, msgName, SeralizeTable( msg ) ) )
	 
	 peer:SendMessage( msgName, code )

end



function SendLoginMessage( msgName, msgTable )
	
	EncodeSend( LoginPeer, msgName, msgTable )

end

local table_insert = table.insert

local function marshal(out, t, indent)

	for k, v in pairs(t) do

		--print("indent",indent, k, v, type(v) )
	
		table_insert(out, k )
		table_insert(out, ": " )
		
		if type(v) == "table" then
			table_insert(out, "{ " )
			out = marshal( out, v, indent + 1 )
			table_insert(out, " }" )
		elseif type(v) == "string" then
			table_insert(out, "\"" )
			table_insert(out, v )
			table_insert(out, "\"" )
		else
			table_insert(out, tostring(v) )
		end
		
		table_insert(out, " ")
		
	end
	
	return out

end


function SeralizeTable( t )

	if type(t) ~= "table" then
		return ""
	end

	local out = marshal( {}, t, 0 )
	
	return table.concat( out, "")

end



--[[
print(SeralizeTable( {
	key = "hello",
	notify = {
		a = 1,
		x = false,
	}
}))

]]
