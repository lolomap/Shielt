using ShieltShared;

ENetManager.InitServer(7777, 2);
LobbyManager lobbyManager = new();

while (true)
{
	ENetManager.Run();
}