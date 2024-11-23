using System;
using System.Net;
using ENet;
using ShieltShared;
using UnityEngine;
using Packet = ShieltShared.Packet;

public class Network : MonoBehaviour
{
	public static Network Instance;

	private bool _isColletedInfo;
	public static string SelfNickname, EnemyNickname;
	public static void SetSelfNickname(string value) => SelfNickname = value; 
    
	public delegate void UpdatePlayersEventHandler(PlayersInfoStC players);

	public static event UpdatePlayersEventHandler UpdatePlayers;
	
	private void Awake()
	{
		Instance = this;
		
		ENetManager.Connected += OnPlayerConnected;
		ENetManager.PacketRecieved += ProcessPackets;
		
		ENetManager.InitClient();
	}

	private void Update()
	{
		if (!ENetManager.IsInited || !ENetManager.IsReady) return;
		ENetManager.Run();
	}

	public void Connect()
	{
		ENetManager.Connect(new(IPAddress.Parse("192.168.39.134"), 7777));
		
		//return ENetManager.IsReady;
	}

	private static void ProcessPackets(Packet packet, Peer peer)
	{
		switch (packet.TypeId)
		{
			case PlayersInfoStC.TYPE_ID:
			{
				PlayersInfoStC payload = PacketManager.UnpackPayload<PlayersInfoStC>(packet);

				Debug.Log("PL1 HP: "+payload.Player1Health);
				Debug.Log("PL2 HP: "+payload.Player2Health);
				
				if (Instance._isColletedInfo)
					UpdatePlayers?.Invoke(payload);
				else
				{
					SelfNickname = payload.Player1Nickname != SelfNickname ? payload.Player1Nickname : payload.Player2Nickname;

					Instance._isColletedInfo = true;
				}
				
				break;
			}
		}
	}

	private static void OnPlayerConnected()
	{
		ConnectionRequestCtS info = new() {Nickname = SelfNickname};
		byte[] data = PacketManager.Pack(info);
		ENetManager.Server.Send(0, data, PacketFlags.None);
	}

	public void RequestAction(int value, bool isDefend)
	{
		PlayerActionCtS info = new() {IsDefend = isDefend, Value = value};
		byte[] data = PacketManager.Pack(info);
		
		ENetManager.Server.Send(0, data, PacketFlags.Reliable);
	}
}