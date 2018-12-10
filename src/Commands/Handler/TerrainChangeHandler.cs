using ColossalFramework;
using CSM.Networking;
using UnityEngine;
using Harmony;
using System.Reflection;

namespace CSM.Commands.Handler
{
    public class TerrainChangeHandler :CommandHandler<TerrainChangeCommand>
    {
        private MethodInfo _UpdateAreaImplementation;

        public TerrainChangeHandler()
        {
            _UpdateAreaImplementation = typeof(TerrainModify).GetMethod("UpdateAreaImplementation", AccessTools.all);
        }

        public override byte ID => CommandIds.TerrainChangeCommand;

        public override void HandleOnServer(TerrainChangeCommand command, Player player) => Handle(command);

        public override void HandleOnClient(TerrainChangeCommand command) => Handle(command);

        private void Handle(TerrainChangeCommand command)
        {
            TerrainManager instance = Singleton<TerrainManager>.instance;
            TerrainArea heightsModified = instance.m_heightsModified;
            TerrainArea surfaceModified = instance.m_surfaceModified;
            TerrainArea zonesModified = instance.m_zonesModified;
            int num = ((command.maxX - command.minX) + 1) * ((command.maxZ - command.minZ) + 1);
            if (instance.m_modifyingLevel != 0)
            {
                if (instance.m_modifyingHeights && command.heights)
                {
                    int num2 = ((heightsModified.m_maxX - heightsModified.m_minX) + 1) * ((heightsModified.m_maxZ - heightsModified.m_minZ) + 1);
                    int num3 = ((Mathf.Max(command.maxX, heightsModified.m_maxX) - Mathf.Min(command.minX, heightsModified.m_minX)) + 1) * ((Mathf.Max(command.maxZ, heightsModified.m_maxZ) - Mathf.Min(command.minZ, heightsModified.m_minZ)) + 1);
                    if ((num3 > ((num2 + num) << 1)) || (num3 > 0x2710))
                    {
                        //I dont think this is the correct way to activate the private methodé AreaImplementation called by the UpdateArea method
                        _UpdateAreaImplementation.Invoke(instance, new object[] { });
                    }
                }
                if (instance.m_modifyingSurface && command.surface)
                {
                    int num4 = ((surfaceModified.m_maxX - surfaceModified.m_minX) + 1) * ((surfaceModified.m_maxZ - surfaceModified.m_minZ) + 1);
                    int num5 = ((Mathf.Max(command.maxX, surfaceModified.m_maxX) - Mathf.Min(command.minX, surfaceModified.m_minX)) + 1) * ((Mathf.Max(command.maxZ, surfaceModified.m_maxZ) - Mathf.Min(command.minZ, surfaceModified.m_minZ)) + 1);
                    if ((num5 > ((num4 + num) << 1)) || (num5 > 0x2710))
                    {
                        _UpdateAreaImplementation.Invoke(instance, new object[] { });
                        //UpdateAreaImplementation();
                    }
                }
                if (instance.m_modifyingZones && command.zones)
                {
                    int num6 = ((zonesModified.m_maxX - zonesModified.m_minX) + 1) * ((zonesModified.m_maxZ - zonesModified.m_minZ) + 1);
                    int num7 = ((Mathf.Max(command.maxX, zonesModified.m_maxX) - Mathf.Min(command.minX, zonesModified.m_minX)) + 1) * ((Mathf.Max(command.maxZ, zonesModified.m_maxZ) - Mathf.Min(command.minZ, zonesModified.m_minZ)) + 1);
                    if ((num7 > ((num6 + num) << 1)) || (num7 > 0x2710))
                    {
                        _UpdateAreaImplementation.Invoke(instance, new object[] { });
                        //UpdateAreaImplementation();
                    }
                }
            }
            instance.m_modifyingHeights = instance.m_modifyingHeights || command.heights;
            instance.m_modifyingSurface = instance.m_modifyingSurface || command.surface;
            instance.m_modifyingZones = instance.m_modifyingZones || command.zones;
            if (command.heights)
            {
                heightsModified.AddArea(command.minX, command.minZ, command.maxX, command.maxZ);
                int num8 = ((heightsModified.m_maxX - heightsModified.m_minX) << 2) + 1;
                int num9 = ((heightsModified.m_maxZ - heightsModified.m_minZ) << 2) + 1;
                if ((num8 * num9) > instance.m_tempHeights.Length)
                {
                    heightsModified.m_maxX = Mathf.Min(heightsModified.m_maxX, (heightsModified.m_minX + 120) + 8);
                    heightsModified.m_maxZ = Mathf.Min(heightsModified.m_maxZ, (heightsModified.m_minZ + 120) + 8);
                }
            }
            if (command.surface)
            {
                surfaceModified.AddArea(command.minX, command.minZ, command.maxX, command.maxZ);
                int num10 = ((surfaceModified.m_maxX - surfaceModified.m_minX) << 2) + 1;
                int num11 = ((surfaceModified.m_maxZ - surfaceModified.m_minZ) << 2) + 1;
                if ((num10 * num11) > instance.m_tempSurface.Length)
                {
                    surfaceModified.m_maxX = Mathf.Min(surfaceModified.m_maxX, (surfaceModified.m_minX + 120) + 8);
                    surfaceModified.m_maxZ = Mathf.Min(surfaceModified.m_maxZ, (surfaceModified.m_minZ + 120) + 8);
                }
            }
            if (command.zones)
            {
                zonesModified.AddArea(command.minX, command.minZ, command.maxX, command.maxZ);
                int num12 = ((zonesModified.m_maxX - zonesModified.m_minX) << 2) + 1;
                int num13 = ((zonesModified.m_maxZ - zonesModified.m_minZ) << 2) + 1;
                if ((num12 * num13) > instance.m_tempZones.Length)
                {
                    zonesModified.m_maxX = Mathf.Min(zonesModified.m_maxX, (zonesModified.m_minX + 120) + 8);
                    zonesModified.m_maxZ = Mathf.Min(zonesModified.m_maxZ, (zonesModified.m_minZ + 120) + 8);
                }
            }
            if ((instance.m_modifyingLevel == 0) || (num > 0x2710))
            {
                _UpdateAreaImplementation.Invoke(instance, new object[] { });
                //UpdateAreaImplementation();
            }

        }



    }
}
