using ProtoBuf;

namespace CSM.Commands
{
    [ProtoContract]
    public class TerrainChangeCommand : CommandBase
    {
        [ProtoMember(1)]
        public int minX;

        [ProtoMember(2)]
        public int minZ;

        [ProtoMember(3)]
        public int maxX;

        [ProtoMember(4)]
        public int maxZ;

        [ProtoMember(5)]
        public bool heights;

        [ProtoMember(6)]
        public bool surface;

        [ProtoMember(7)]
        public bool zones;

    }
}
