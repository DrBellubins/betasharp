using System.IO;
using BetaSharp.Network.Packets;
using BetaSharp.Network.Packets;
using java.io;

namespace BetaSharp.Network.Packets.S2CPlay
{
    public class PlayerAbilitiesS2CPacket : Packet
    {
        public bool CanFly { get; set; }
        public bool IsFlying { get; set; }
        // Optionally, add more abilities (NoClip, FlySpeed, etc.)

        public PlayerAbilitiesS2CPacket() { }

        public PlayerAbilitiesS2CPacket(bool canFly, bool isFlying)
        {
            CanFly = canFly;
            IsFlying = isFlying;
        }

        public override void read(DataInputStream reader)
        {
            CanFly = reader.readInt() == 1;
            IsFlying = reader.readInt() == 1;
        }

        public override void write(DataOutputStream writer)
        {
            writer.write(CanFly ? 1 : 0);
            writer.write(IsFlying ? 1 : 0);
        }

        public override void apply(NetHandler handler)
        {
            handler.HandlePlayerAbilities(this);
        }

        public override int size() => 8;
    }
}
