﻿using ColossalFramework;
using CSM.Networking;

namespace CSM.Commands.Handler
{
    public class ZoneHandler : CommandHandler<ZoneCommand>
    {
        public override byte ID => CommandIds.ZoneCommand;

        public override void HandleOnServer(ZoneCommand command, Player player) => Handle(command);

        public override void HandleOnClient(ZoneCommand command) => Handle(command);

        private void Handle(ZoneCommand command)
        {
            var id = Extensions.ZoneExtension.ZoneVectorDictionary[command.Position];

            UnityEngine.Debug.Log($"Zoneid changed is {id}");
            Singleton<ZoneManager>.instance.m_blocks.m_buffer[id].m_zone1 = command.Zone1;
            Singleton<ZoneManager>.instance.m_blocks.m_buffer[id].m_zone2 = command.Zone2;

            // This Command is necessary to get the Zone to render, else it will first show when a building is build on it.
            Singleton<ZoneManager>.instance.m_blocks.m_buffer[id].RefreshZoning(id);
        }
    }
}