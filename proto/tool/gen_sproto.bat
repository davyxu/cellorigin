sprotogen.exe ^
--go_out=..\..\server\src\proto\msg_sp.go ^
--cs_out=..\..\client\Assets\Script\Proto\sp.cs ^
--package=proto ^
--cellnet_reg=true ^
%*

@IF %ERRORLEVEL% NEQ 0 pause