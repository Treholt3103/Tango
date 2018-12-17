using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class BuildingUpgradeHandler : CommandHandler<BuildingUpgradeCommand>
    {
        public override byte ID => CommandIds.BuildingUpgradeCommand;

        public override void HandleOnServer(BuildingUpgradeCommand command, Player player) => HandleBuilding(command);

        public override void HandleOnClient(BuildingUpgradeCommand command) => HandleBuilding(command);

        private void HandleBuilding(BuildingUpgradeCommand command)
        {
            //Injections.UpgradeBuilding.ignoreUpgrade = true;
            BuildingManager.instance.UpgradeBuilding(command.BuildingId, false);
            //Injections.UpgradeBuilding.ignoreUpgrade = false;
        }

    }
}
