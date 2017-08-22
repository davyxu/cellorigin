package meta

type FileDescriptorSet struct {
	Files []*FileDescriptor

	unknownFields []*lazyField
}

func (self *FileDescriptorSet) resolveAll() error {

	for _, v := range self.unknownFields {
		if _, err := v.resolve(2); err != nil {
			return err
		}
	}

	return nil
}

func (self *FileDescriptorSet) addFile(file *FileDescriptor) {
	file.FileSet = self
	self.Files = append(self.Files, file)
}

func (self *FileDescriptorSet) parseType(name string) (ft FieldType, structType *Descriptor) {

	for _, file := range self.Files {

		if ft, structType = file.rawParseType(name); ft != FieldType_None {
			return ft, structType
		}
	}

	return FieldType_None, nil
}

func NewFileDescriptorSet() *FileDescriptorSet {
	return &FileDescriptorSet{}
}
