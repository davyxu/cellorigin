package main

import (
	"fmt"
	"table"
)

func main() {

	table.LoadServiceTable()

	for name, def := range table.ServiceNameMap {
		fmt.Println(name, def)
	}

}
