using ProtoBuf;

namespace CSM.Commands
{
    [ProtoContract]
    public class BuildingUpgradeCommand : CommandBase
    {
        [ProtoMember(1)]
        public ushort BuildingId { get; set; }
    }
}
