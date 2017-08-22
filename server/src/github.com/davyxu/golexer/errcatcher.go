package golexer

func ErrorCatcher(errFunc func(error)) {

	err := recover()

	switch err.(type) {
	// 运行时错误
	case interface {
		RuntimeError()
	}:

		// 继续外抛， 方便调试
		panic(err)

	case error:
		errFunc(err.(error))
	case nil:
	default:
		panic(err)
	}
}
