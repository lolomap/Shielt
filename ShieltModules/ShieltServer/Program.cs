using ShieltShared;

Log.Init();
ENetManager.InitServer(7777, 2);
LobbyManager lobbyManager = new();

while (true)
{
	ENetManager.Run();
}