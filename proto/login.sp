

// client -> login
message LoginREQ {

	PlatformToken string
}

// login -> client
message LoginACK {
	
	Result int32
		
	Token string
}

