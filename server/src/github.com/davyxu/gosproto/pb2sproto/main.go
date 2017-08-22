package main

import (
	"flag"
	"fmt"
	"os"

	"github.com/davyxu/pbmeta"
)

var paramOut = flag.String("outdir", "", "output directory")

var paramPBMeta = flag.String("pbmeta", "", "Input Google Protobuf Descripte Binary file format, use protoc generated!")

func getPbMeta(filename string) (*pbmeta.DescriptorPool, error) {

	// 请先运行ExportPluginMeta导出test.pb
	fds, err := pbmeta.LoadFileDescriptorSet(filename)

	if err != nil {
		return nil, err
	}

	// 描述池
	return pbmeta.NewDescriptorPool(fds), nil

}

func main() {

	flag.Parse()

	pool, err := getPbMeta(*paramPBMeta)
	if err != nil {
		fmt.Println(err)
		os.Exit(1)
	}

	for fileIndex := 0; fileIndex < pool.FileCount(); fileIndex++ {

		gen_proto(pool.File(fileIndex), *paramOut)

	}

}
