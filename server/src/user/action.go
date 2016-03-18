package user

type actionData struct {
	f func(...interface{})

	next *actionData
}

// 类似于C#的Action, 一种delegate
type Action struct {
	link *actionData
}

func (self *Action) Invoke(args ...interface{}) {

	link := self.link

	for link != nil {
		link.f(args...)

		link = link.next
	}

}

func (self *Action) Add(callback func(...interface{})) {

	self.link = &actionData{callback, self.link}
}

func (self *Action) Clear() {
	self.link = nil
}
