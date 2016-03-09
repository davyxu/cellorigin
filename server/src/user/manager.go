package user

type Manager struct {
	userMap map[int64]*User
}

func (self *Manager) Add(p *User) {
	self.userMap[p.ID()] = p
}

func (self *Manager) Remove(id int64) {
	delete(self.userMap, id)
}

func (self *Manager) GetByID(id int64) *User {
	if p, ok := self.userMap[id]; ok {
		return p
	}

	return nil
}

func NewManager() *Manager {
	return &Manager{
		userMap: make(map[int64]*User),
	}
}
