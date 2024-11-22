namespace ShieltShared
{
	public static class PacketManager
	{
		public static byte[] Pack<T>(T payload) where T : struct, IPacketPayload
		{
			Packet packet = new()
			{
				TypeId = payload.GetId(),
				Payload = payload.PackReflection()
			};

			return packet.Pack();
		}

		public static Packet UnpackPacket(byte[] data)
		{
			Packet result = new();
			result.Unpack(data);
			return result;
		}

		public static T UnpackPayload<T>(Packet packet) where T : struct, IPacketPayload
		{
			T result = new();
			result.UnpackReflection(packet.Payload);

			return result;
		}

		public static T UnpackPayload<T>(byte[] data) where T : struct, IPacketPayload
		{
			T result = new();
			result.UnpackReflection(data);

			return result;
		}
	}
}