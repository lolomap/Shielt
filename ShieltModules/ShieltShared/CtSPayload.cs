using StructPacker;

namespace ShieltShared
{
//******** CLIENT TO SERVER PACKETS **********\\
// Use CtS postfix in names for difference

	[Pack]
	public struct PlayerActionCtS : IPacketPayload
	{
		[SkipPack] public const ushort TYPE_ID = 0;
		public ushort GetId() => TYPE_ID;
		public byte[] PackReflection() => this.Pack();
		public void UnpackReflection(byte[] data) => this.Unpack(data);

		public bool IsDefend;
		public int Value;
	}

	[Pack]
	public struct ConnectionRequestCtS : IPacketPayload
	{
		[SkipPack] public const ushort TYPE_ID = 1;
		public ushort GetId() => TYPE_ID;
		public byte[] PackReflection() => this.Pack();
		public void UnpackReflection(byte[] data) => this.Unpack(data);

		public string Nickname;
	}
}