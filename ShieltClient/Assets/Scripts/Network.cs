﻿using System;
using System.Net;
using ENet;
using ShieltShared;
using UnityEngine;
using Packet = ShieltShared.Packet;

public class Network : MonoBehaviour
{
	public static Network Instance;
	
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
		ENetManager.Connect(new(IPAddress.Parse("127.0.0.1"), 7777));
		
		//return ENetManager.IsReady;
	}

	private static void ProcessPackets(Packet packet, Peer peer)
	{
		switch (packet.TypeId)
		{
			case PlayersInfoStC.TYPE_ID:
			{
				PlayersInfoStC payload = PacketManager.UnpackPayload<PlayersInfoStC>(packet);

				Debug.Log("PL1: "+payload.Player1Health.ToString());
				Debug.Log("PL2: "+payload.Player2Health.ToString());
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

	public void RequestAction(int value)
	{
		PlayerActionCtS info = new() {IsDefend = value < 0, Value = Math.Abs(value)};
		byte[] data = PacketManager.Pack(info);
		
		ENetManager.Server.Send(0, data, PacketFlags.Reliable);
	}
}