package meta

import "strings"

func parseFileTag(p *sprotoParser, fileD *FileDescriptor, srcName string) {

	p.Expect(Token_FileTag)

	rawTagStr := p.Expect(Token_String).Value()
	for _, tagStr := range strings.Split(rawTagStr, " ") {
		fileD.fileTag = append(fileD.fileTag, strings.TrimSpace(tagStr))
	}

}
