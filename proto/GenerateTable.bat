..\tool\protoc.exe --plugin=protoc-gen-table=..\tool\bin\protoc-gen-table.exe ^
--table_out ..\server\src\table ^
--proto_path "." ^
behavior.proto