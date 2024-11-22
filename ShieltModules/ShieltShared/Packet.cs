using StructPacker;

namespace ShieltShared
{
	[Pack]
	public struct Packet
	{
		public ushort TypeId;
		public byte[] Payload;
	}


	/// <summary>
	/// Payload structures must implement this interface for universal access to TYPE_ID const,
	/// source generated Pack/Unpack methods
	/// </summary>
	public interface IPacketPayload
	{
		public ushort GetId();

		public byte[] PackReflection();
		public void UnpackReflection(byte[] data);
	}
}