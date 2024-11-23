using StructPacker;

namespace ShieltShared
{
//******** SERVER TO CLIENT PACKETS **********\\
// Use StC postfix in names for difference

	[Pack]
	public struct PlayersInfoStC : IPacketPayload
	{
		[SkipPack] public const ushort TYPE_ID = 1001;
		public ushort GetId() => TYPE_ID;
		public byte[] PackReflection() => this.Pack();
		public void UnpackReflection(byte[] data) => this.Unpack(data);

		public int Player1Health;
		public int Player2Health;
		
		public bool Player1IsDefend;
		public bool Player2IsDefend;

		public string Player1Nickname;
		public string Player2Nickname;
	}
}