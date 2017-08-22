cd ..
set GOPATH=%cd%
go install -gcflags "-N -l" -v svcmod/login

