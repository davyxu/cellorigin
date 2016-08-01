Network ={}



function Network.Init( )

	LuaPB.RegisterFile("Assets/game.pb")
	
	LoginPeer = PeerManagerLua.Instance:Get( "login" )
	
	GamePeer = PeerManagerLua.Instance:Get( "game" )
	
end

-- C#收到消息后, 通过Dispatcher找到注册的handler, 回调此函数
function Network.DecodeRecv( peer, msgName, stream, callback )


	if stream == nil then

		if peer.DebugMessage then
			print(string.format("[%s] #recv %s", peer.Name, msgName ) )
		end
		
		callback( )
	
	else
	
		local msg = luapb_decode( msgName, stream )
		
		if peer.DebugMessage then
			print(string.format("[%s] #recv %s|%s", peer.Name, msgName, SeralizeTable( msg ) ) )
		end
		
		callback( msg )
		
	end

end


local function EncodeSend( peer, msgName, msgTable )

	 local stream = luapb_encode(msgName, msgTable)
	 
	 if peer.DebugMessage then
		print(string.format("[%s] #send %s|%s", peer.Name, msgName, SeralizeTable( msgmsgTable) ) )
	 end
	 
	 peer:SendMessage( msgName, stream )

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
