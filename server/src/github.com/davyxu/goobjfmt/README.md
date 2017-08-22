# goobjfmt

Marshal struct input text format( protobuf text )

like 
	key : value
	
	key has no ""
	
	string value should have ""

# Example

```golang
	input := &MyData{
		Name:   "genji",
		Type:   MyCar_Pig,
		Uint32: math.MaxUint32,
		Int64:  math.MaxInt64,
		Uint64: math.MaxUint64,
	}

	t.Log(MarshalTextString(input))
```

# Feedback

Star me if you like or use, thanks

blog(Chinese): [http://www.cppblog.com/sunicdavy](http://www.cppblog.com/sunicdavy)

zhihu(Chinese): [http://www.zhihu.com/people/sunicdavy](http://www.zhihu.com/people/sunicdavy)

