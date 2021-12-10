@echo off
cd..
set "protopath=%cd%"
cd protocol
protoc --proto_path=./proto --cpp_out=out "./proto/KyCmdProtocol.proto"
protoc --proto_path=./proto --cpp_out=out "./proto/KyMsgProtocol.proto"
pause