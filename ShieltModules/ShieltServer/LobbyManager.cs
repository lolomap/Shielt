using System.Net;
using ENet;
using Serilog.Core;
using ShieltShared;
using Packet = ShieltShared.Packet;

public class Player
{
	public Peer? Connection;
	
	public int Health;
	public PlayerActionCtS? LastAction;
}

public class LobbyManager
{
	private readonly Player _player1 = new();
	private readonly Player _player2 = new();

	public LobbyManager()
	{
		ENetManager.PacketRecieved += OnPacketRecieved;
	}

	public void Dispose()
	{
		ENetManager.PacketRecieved -= OnPacketRecieved;
	}

	~LobbyManager()
	{
		Dispose();
	}

	private void OnPacketRecieved(Packet packet, Peer peer)
	{
		switch (packet.TypeId)
		{
			case ConnectionRequestCtS.TYPE_ID:
				OnConnection(peer);
				break;
			case PlayerActionCtS.TYPE_ID:
				OnPlayerAction(PacketManager.UnpackPayload<PlayerActionCtS>(packet), peer);
				break;
		}
	}

	private void OnConnection(Peer peer)
	{
		if (_player1.Connection == null) {_player1.Connection = peer; return;}

		_player2.Connection ??= peer;
		
		Log.Logger.Warning("Third player tried to connect!");
		peer.DisconnectLater(0);
	}

	private void OnPlayerAction(PlayerActionCtS action, Peer peer)
	{
		if (_player1.Connection == null || _player2.Connection == null) 
			return;
		
		Player player = ENetManager.PeerEquals(peer, _player1.Connection.Value) ? _player1 : _player2;
		player.LastAction = action;

		if (_player1.LastAction == null || _player2.LastAction == null) return;
		
		
		int player1Attack = 0, player2Attack = 0, player1Defend = 0, player2Defend = 0;

		if (_player1.LastAction.Value.IsDefend) player1Defend = _player1.LastAction.Value.Value;
		else player1Attack = _player1.LastAction.Value.Value;
		if (_player2.LastAction.Value.IsDefend) player2Defend = _player1.LastAction.Value.Value;
		else player2Attack = _player1.LastAction.Value.Value;

		_player1.Health -= Math.Clamp(player2Attack - player1Defend, 0, 100);
		_player2.Health -= Math.Clamp(player1Attack - player2Defend, 0, 100);

		_player1.LastAction = null;
		_player2.LastAction = null;

		PlayersInfoStC playerInfo;
		playerInfo.Player1Health = _player1.Health;
		playerInfo.Player2Health = _player2.Health;
		byte[] data = PacketManager.Pack(playerInfo);
		ENetManager.Broadcast(0, data, PacketFlags.Reliable);
	}
}