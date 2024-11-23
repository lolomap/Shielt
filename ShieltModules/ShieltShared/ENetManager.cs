using System.Net;
using ENet;


namespace ShieltShared
{
	public static class ENetManager
	{
		private static Host _host;
		private static Event _event;

		public static bool IsInited { get; private set; }
		public static bool IsReady { get; private set; }
		public static bool IsServer { get; private set; }
		public static bool IsConnected { get; private set; }
		public static Peer Server { get; private set; }

		public static readonly Queue<Packet> RecievedPackets = new();

		/// <summary>
		/// Is true if no events occured in last tick
		/// </summary>
		public static bool Idle { get; private set; }

		public delegate void PacketRecievedHandler(Packet packet, Peer peer);

		public static event PacketRecievedHandler PacketRecieved;

		public delegate void ConnectedHandler();

		public static event ConnectedHandler Connected;

		public delegate void DisconnectedHandler();

		public static event DisconnectedHandler Disconnected;


		public static void InitServer(int port, int maxPeers)
		{
			if (IsInited) throw new MethodAccessException();

			_host = new();
			_host.InitializeServer(port, maxPeers);

			IsServer = true;
			IsInited = true;
			IsReady = true;

			Console.WriteLine("Networking is ready");
		}

		public static void InitClient()
		{
			if (IsInited) throw new MethodAccessException();

			_host = new();
			_host.InitializeClient(1);

			IsInited = true;

			Console.WriteLine("Networking is ready");
		}

		public static void Connect(IPEndPoint address)
		{
			if (!IsInited) throw new MethodAccessException();
			if (IsServer) throw new MethodAccessException();
			if (IsConnected) throw new MethodAccessException();

			IsReady = true;
			Server = _host.Connect(address, 0);
		}

		public static void Run()
		{
			Idle = !_host.Service(0, out _event);
			switch (_event.Type)
			{
				case EventType.Connect:
					if (!IsServer) IsConnected = true;

					Connected?.Invoke();

					Console.WriteLine("Peer connected");
					break;

				case EventType.Disconnect:
					if (!IsServer) IsConnected = false;

					Disconnected?.Invoke();
					Console.WriteLine("Peer disconnected");
					break;

				case EventType.Receive:
					byte[] data = _event.Packet.GetBytes();

					Packet packet = PacketManager.UnpackPacket(data);
					//RecievedPackets.Enqueue(packet);
					PacketRecieved?.Invoke(packet, _event.Peer);

					_event.Packet.Dispose();
					break;

				default:
				case EventType.None:
					break;
			}
		}

		public static void Broadcast(byte channel, byte[] data, PacketFlags flags)
		{
			if (!IsServer) throw new MethodAccessException();

			ENet.Packet packet = new();
			packet.Initialize(data, 0, data.Length, flags);
			ref ENet.Packet toSend = ref packet;
			_host.Broadcast(channel, ref toSend);
		}

		public static bool PeerEquals(Peer p1, Peer p2)
		{
			return p1.GetRemoteAddress().Equals(p2.GetRemoteAddress());
		}

		public static bool PeerHasAddress(Peer peer, IPEndPoint address)
		{
			return peer.GetRemoteAddress().Equals(address);
		}

		public static void Dispose()
		{
			if (!IsServer && IsConnected) Server.DisconnectLater(0);
			_host.Flush();
			_host.Dispose();
		}
	}
}