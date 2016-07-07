Network ={}

require "pbc/protobuf"

function Network.Init( pbfile )

	protobuf.register_file(pbfile)

end


function Network.DecodeRecv( msgName, stream, callback )

	local msg, err = protobuf.decode( msgName, stream )
	
	if msg == false then
		logError(msgName .. err)
		return
	end
	
	callback( msg )
	
end

function Network.EncodeSend( peer, msgName, msgTable )

	 local code = protobuf.encode(msgName, msgTable)
	 
	 peer:SendMessage( msgName, code )

end