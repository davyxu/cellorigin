// extend standard


message PhoneNumber {

	// 头注释
	number string // 尾注释

    // 整形
	type int32
		
}


message Person {
	
	name string
		
	id int32
		
	email string
		
	phone []PhoneNumber
		
}


message AddressBook {
	
	person []Person
		
}


enum MyCar {
	
	Monkey
		
	Monk
		
	Pig
		
}


message MyData {
	
	name  string
		
	Type  MyCar
		
	Int32  int32    // extend standard

	Uint32  uint32
		
	Int64  int64
		
	Uint64  uint64
		
	Bool  bool

	Float32 float32     // [ExtendPrecision]100  # 手动设置精度, 默认1000

	Float64 float64

	Stream bytes
		
}


message MyProfile {
	
	nameField MyData
		
	nameArray []MyData
		
	nameMap []MyData(Type)
		
}


