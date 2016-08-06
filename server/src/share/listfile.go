package share

import (
	"bufio"
	"os"
	"strings"
)

type ListFile struct{}

func (self *ListFile) Load(fileName string, doLine func(strline string)) (int32, error) {
	f, err := os.Open(fileName)
	var lineCount int32
	defer f.Close()

	if err == nil {
		reader := bufio.NewReader(f)

		for {

			line, isprefix, err := reader.ReadLine()

			if err != nil {
				break
			}

			if !isprefix && doLine != nil {

				doLine(strings.TrimSpace(string(line)))

				lineCount++
			}

		}

	} else {
		log.Errorf("open file error %s", fileName)
		return 0, err
	}

	return lineCount, nil
}
