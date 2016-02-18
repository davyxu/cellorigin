package table

import (
	"proto/gamedef"
	"share"
)

func GetServiceByID(name string) *gamedef.ServiceDefine {

	if def, ok := serviceMap[name]; ok {
		return def
	}

	return nil
}

var serviceMap = make(map[string]*gamedef.ServiceDefine)

var servicefile gamedef.ServiceFile

func LoadServiceTable() {

	log.Infoln("load service table...")

	if share.LoadTable("Service", &file) != nil {
		return
	}

	for _, def := range file.Service {

		name := def.Name

		if GetServiceByID(name) != nil {
			log.Errorf("duplicate service name: %d", name)
			continue
		}

		serviceMap[name] = def
	}

}
