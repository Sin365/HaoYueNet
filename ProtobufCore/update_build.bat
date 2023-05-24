call proto2cpp.exe protoc.exe
cd..
set "protopath=%cd%"
cd new_server_proto
copy %cd%\out\cpp\ %protopath%\newhxserver\ProtocolSrc\
pause