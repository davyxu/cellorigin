..\tool\protoc.exe --plugin=protoc-gen-meta=..\tool\protoc-gen-meta.exe --proto_path ..\proto --meta_out=.\game.pb:. ^
..\proto\channel.proto ^
..\proto\service.proto



..\tool\tabtoy.exe --mode=xls2pbt --pb=.\game.pb --outdir=..\server\src\cfg --para=true --haltonerr=true ^
Channel.xlsx ^
Service.xlsx