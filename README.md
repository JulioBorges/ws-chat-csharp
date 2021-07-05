# WS Chat CSharp

The pourpose of this project is implement a Web Chat using WebSockets with C#.

## Structure
### WSChat.Shared
Shared library to share components between projects

### WSChat.Server
WebSocket Chat server, it's necessary to run to be able start a server.
#### -- Initialization --
It's possible to start server passing IP and Port as arguments:

<code>WSChat.Server.exe 127.0.0.1 54000</code>

If thoose informations not passed, it's possible to pass IP and Port by console.

### WSChat.Client
WebSocket Chat client, it's necessary to run, to be able start a server.
#### -- Initialization --
Simillary Server, it's possible to start client passing IP and Port as arguments:

<code>WSChat.Client.exe 127.0.0.1 54000</code>

If thoose informations not passed, it's possible to pass IP and Port by console.
