: 使用方法call build.bat your\path\to\opensource
: opensource 为github.com的依赖库
set GOPATH=%cd%;%1%
go install -p 4 -v gamesvc
go install -p 4 -v agentsvc
go install -p 4 -v loginsvc