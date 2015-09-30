# real-time-ticker
Real-time-ticker support for NeTV and other clients.

Pub-sub (ZMQ)
Websockets (Fleck)
NeTV via HTTP API for now.

## Note requires Visual Studio 2015 and .NET 4.5.2

## design-intent
- Prove that C# solution can coexist successfully in both Win32/64 as well as Linux worlds
- Less, but better: lightweight components that build up to a robust and highly functional server
- Full service client libraries: library support for rich client apps to connect to server
- Agnostic client support: fully support http://, ws:// and tcp:// via JSON-based contracts
