using ColossalFramework;
using CSM.Commands;
using Harmony;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CSM.Injections
{
    public class TransportHandler
    {
        public static List<ushort> IgnoreLines { get; } = new List<ushort>();
        public static List<ushort> IgnoreStops { get; } = new List<ushort>();
    }

    [HarmonyPatch(typeof(TransportManager))]
    [HarmonyPatch("CreateLine")]
    public class CreateLine
    {
        public static void Postfix(bool __result, ref ushort lineID, bool newNumber)
        {
            UnityEngine.Debug.Log("line was created");
            if (__result)
            {
                Command.SendToAll(new TransportCreateLineCommand
                {
                    lineID = lineID,
                    newNumber = newNumber,
                    infoIndex = TransportManager.instance.m_lines.m_buffer[lineID].m_infoIndex,
                });
            }
        }
    }

    [HarmonyPatch(typeof(TransportManager))]
    [HarmonyPatch("ReleaseLineImplementation")]
    public class ReleaseLineImplementation
    {
        public static void Prefix(ushort lineID, ref TransportLine data)
        {

            if (data.m_flags != 0 && !TransportHandler.IgnoreLines.Contains(lineID))
            {
                Command.SendToAll(new TransportReleaseLineCommand
                {
                    LineID = lineID,
                });
            }
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    [HarmonyPatch("AddStop")]
    public class AddStod
    {
        public static void Postfix(bool __result, ushort lineID, int index, Vector3 position, bool fixedPlatform)
        {
            if (__result)
            {
                Command.SendToAll(new TransportLineAddStopCommand
                {
                    fixedPlatform = fixedPlatform,
                    LineID = lineID,
                    index = index,
                    Position = position
                });
            }

            UnityEngine.Debug.Log("stop was added");
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    public class RemoveStop
    {
        public static void Postfix(ushort lineID, int index, bool __result)
        {
            if (__result)
            {
                UnityEngine.Debug.Log("stop was removed");
                Command.SendToAll(new TransportLineRemoveStopCommand
                {
                    LineID = lineID,
                    Index = index,
                });

            }
        }
        public static MethodBase TargetMethod()
        {
            return typeof(TransportLine).GetMethod("RemoveStop", AccessTools.all, null, new Type[] { typeof(ushort), typeof(int), typeof(Vector3).MakeByRefType() }, new ParameterModifier[] { });
        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    public class MoveStop
    {
        public static void Postfix(ushort lineID, int index, Vector3 newPos, bool fixedPlatform, bool __result)
        {
            UnityEngine.Debug.Log("stop was moved");
            if (__result)
            {
                Command.SendToAll(new TransportLineMoveStopCommand
                {
                    LineID = lineID,
                    index = index,
                    newPosition = newPos,
                    fixedPlatform = fixedPlatform
                });
            }
        }

        public static MethodBase TargetMethod()
        {
            return typeof(TransportLine).GetMethod("MoveStop", AccessTools.all, null, new Type[] { typeof(ushort), typeof(int), typeof(Vector3), typeof(bool), typeof(Vector3).MakeByRefType() }, new ParameterModifier[] { });
        }

    }
}