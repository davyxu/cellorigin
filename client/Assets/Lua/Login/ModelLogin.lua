
Model.Define( "ModelLogin",{ 
	Account = "t1",
	Address = "127.0.0.1:8101",
})

-- 常量通过metatable进行限定
LoginConstant = 
{
	DevAddress = "127.0.0.1:8101",
	PublicAddress = "www.test.com:8101",
}