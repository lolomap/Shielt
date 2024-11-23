using System;
using System.Net;
using ENet;
using ShieltShared;
using UnityEngine;
using Packet = ShieltShared.Packet;

public class Network : MonoBehaviour
{
	private void Awake()
	{
		ENetManager.Connected += OnPlayerConnected;
		ENetManager.PacketRecieved += ProcessPackets;
		
		ENetManager.InitClient();
	}

	private void Update()
	{
		if (!ENetManager.IsInited || !ENetManager.IsReady) return;
		ENetManager.Run();
	}

	public bool Connect()
	{
		ENetManager.Connect(new(IPAddress.Parse("127.0.0.1"), 7777));
		
		return ENetManager.IsReady;
	}

	private static void ProcessPackets(Packet packet, Peer peer)
	{
		switch (packet.TypeId)
		{
			case PlayersInfoStC.TYPE_ID:
			{
				PlayersInfoStC payload = PacketManager.UnpackPayload<PlayersInfoStC>(packet);

				break;
			}
		}
	}

	private static void OnPlayerConnected()
	{
		ConnectionRequestCtS info = new();
		byte[] data = PacketManager.Pack(info);
		ENetManager.Server.Send(0, data, PacketFlags.None);
	}
}