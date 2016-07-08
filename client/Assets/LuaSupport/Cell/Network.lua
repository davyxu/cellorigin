Network ={}

require "pbc/protobuf"

function Network.Init( pbfile )

	protobuf.register_file(pbfile)
	
	LoginPeer = PeerManager.Instance:Get( "login" )

end


function Network.DecodeRecv( msgName, stream, callback )

	if stream == nil then

		callback( )
	
	else
	
		local msg, err = protobuf.decode( msgName, stream )
	
		if msg == false then
			logError(msgName .. err)
			return
		end
		
		callback( msg )
		
	end
	
	

	
end

local function EncodeSend( peer, msgName, msgTable )

	 local code = protobuf.encode(msgName, msgTable)
	 
	 peer:SendMessage( msgName, code )

end



function SendLoginMessage( msgName, msgTable )
	
	EncodeSend( LoginPeer, msgName, msgTable )

end