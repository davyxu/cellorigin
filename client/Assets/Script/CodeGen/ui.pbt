CodeGen 
{
	Name: "LoginCharInfo"
	ModelGen: MGT_Instance
}


CodeGen 
{
	Name: "Login"

	Peer
	{
		Name: "login"
	
		RecvMessage: "gamedef.PeerConnected"
		RecvMessage: "gamedef.LoginACK"		
	
	}
	
	Peer
	{
		Name: "game"
	
		RecvMessage: "gamedef.PeerConnected"
		RecvMessage: "gamedef.VerifyGameACK"
	
	}
}

CodeGen 
{
	Name: "LoginCharList"
	Peer
	{
		Name: "game"
	
		RecvMessage: "gamedef.CharListACK"		
	
	}
}

CodeGen 
{
	Name: "LoginServerInfo"
	
	NoGenPresenterCode: true
}



