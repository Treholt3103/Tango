using ColossalFramework;
using CSM.Commands;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSM.Injections
{
    class TerrainHandler
    {
    }
    [HarmonyPatch(typeof(TerrainModify))]
    [HarmonyPatch("UpdateArea")]
    public class UpdateArea
    {
        //This does not seems to be triggered
        public static void Postfix(int minX, int minZ, int maxX, int maxZ, bool heights, bool surface, bool zones)
        {
            UnityEngine.Debug.Log("send terrainChangeCommand");
            Command.SendToAll(new TerrainChangeCommand
            {
                minX = minX,
                minZ = minZ,
                maxX = maxX,
                maxZ = maxZ,
                heights = heights,
                surface = surface,
                zones = zones
            });
        }
    }
}
