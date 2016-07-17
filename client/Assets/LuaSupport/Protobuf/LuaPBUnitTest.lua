

LuaPB.RegisterFile("game.pb")

-- local data = LuaPB.GetTestData()

local stream = luapb_encode( "tutorial.Person", {
	name = "hello",
	test = {1, 2},	
	phone = {

		{number= "789" , type = "WORK" },

		{number= "456" , type = "HOME" },
	},
})

LuaPB.TestStream(stream)

local outdata = luapb_decode( "tutorial.Person", LuaPB.GetTestData( ) )
dump( outdata )