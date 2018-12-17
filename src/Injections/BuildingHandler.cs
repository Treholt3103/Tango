using CSM.Commands;
using Harmony;
using System;


namespace CSM.Injections
{
    public class BuildingHandler
    {
    }

    [HarmonyPatch(typeof(BuildingManager))]
    [HarmonyPatch("UpgradeBuilding")]
    [HarmonyPatch(new Type[] { typeof(bool), typeof(bool) })]
    public class UpgradeBuilding
    {
        //public static bool ignoreUpgrade = false;
        public static void Postfix() //bool __result, ref ushort buildingID
        {
            UnityEngine.Debug.Log("upgrade buildings");
            /*if (!ignoreUpgrade)
            {
                //if (__result)
                //{
                    Command.SendToAll(new BuildingUpgradeCommand
                    {
                        BuildingId = buildingID
                    });
                //}
            //}*/
        }
    }
}
