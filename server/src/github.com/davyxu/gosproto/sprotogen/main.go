package main

import (
	"flag"
	"fmt"
	"os"

	"github.com/davyxu/gosproto/meta"
)

var paramGoOut = flag.String("go_out", "", "golang output filename")
var paramLuaOut = flag.String("lua_out", "", "lua output filename")
var paramEmmyLuaOut = flag.String("emmylua_out", "", "emmy lua output filename")
var paramCSOut = flag.String("cs_out", "", "csharp output filename")
var paramSprotoOut = flag.String("sproto_out", "", "standard sproto output filename")
var paramPackage = flag.String("package", "", "package name in go files")
var paramCellnetReg = flag.Bool("cellnet_reg", false, "for type go, generate sproto auto register entry for github.com/davyxu/cellnet")

//var paramForceAutoTag = flag.Bool("forceatag", false, "no ouput field tag in sp mode")
var paramCSClassAttr = flag.String("cs_classattr", "", "add given string to class header as attribute in c sharp file")
var paramCSFieldAttr = flag.String("cs_fieldattr", "", "add given string to class private field as attribute in c sharp file")
var paramVersion = flag.Bool("version", false, "Show version")

var paramEnumValueGroup = flag.Bool("enumvalgroup", false, "enum value into group")

func mergeSchema(filelist []string) *meta.FileDescriptorSet {

	if len(filelist) == 0 {
		fmt.Println("require sproto file")
		os.Exit(1)
	}

	fileSet := meta.NewFileDescriptorSet()
	errorFileName, err := meta.ParseFileList(fileSet, filelist)
	if err != nil {
		fmt.Println(errorFileName, err.Error())
		os.Exit(1)
	}

	return fileSet
}

const Version = "0.1.0"

func genFile(fileset *meta.FileDescriptorSet, generator func(*fileModel, string), filename string) {

	fm := &fileModel{
		FileDescriptorSet: fileset,
		PackageName:       *paramPackage,
		CellnetReg:        *paramCellnetReg,
		CSClassAttr:       *paramCSClassAttr,
		CSFieldAttr:       *paramCSFieldAttr,
		EnumValueGroup:    *paramEnumValueGroup,
	}

	if *paramEnumValueGroup {
		enumValueGroup(fm)
	}

	generator(fm, filename)
}

func main() {

	flag.Parse()

	// 版本
	if *paramVersion {
		fmt.Println(Version)
		return
	}

	fileset := mergeSchema(flag.Args())

	if *paramGoOut != "" {

		genFile(fileset, gen_go, *paramGoOut)
	}

	if *paramLuaOut != "" {
		genFile(fileset, gen_lua, *paramLuaOut)
	}

	if *paramEmmyLuaOut != "" {
		genFile(fileset, gen_emmylua, *paramEmmyLuaOut)
	}

	if *paramCSOut != "" {
		genFile(fileset, gen_csharp, *paramCSOut)
	}

	if *paramSprotoOut != "" {
		genFile(fileset, gen_sproto, *paramSprotoOut)
	}

}
