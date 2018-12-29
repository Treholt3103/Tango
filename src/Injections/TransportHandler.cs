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
        public static List<ushort> IgnoreAddStops { get; } = new List<ushort>();
        public static List<ushort> IgnoreRemoveStops { get; } = new List<ushort>();
        public static List<ushort> IgnoreMoveStops { get; } = new List<ushort>();

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
            if (!TransportHandler.IgnoreAddStops.Contains(lineID) && !TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary)) //__result && 
            {
                UnityEngine.Debug.Log("stop was added");
                UnityEngine.Debug.Log($"stop has flags {TransportManager.instance.m_lines.m_buffer[lineID].m_flags}");
                Command.SendToAll(new TransportLineAddStopCommand
                {
                    fixedPlatform = fixedPlatform,
                    LineID = lineID,
                    index = index,
                    Position = position
                });
            }

        }
    }

    [HarmonyPatch(typeof(TransportLine))]
    public class RemoveStop
    {
        public static void Postfix(ushort lineID, int index, bool __result)
        {
            if (!TransportHandler.IgnoreRemoveStops.Contains(lineID) && !TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary)) //__result && 
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
            UnityEngine.Debug.Log($"stop has fixed platform {fixedPlatform}");
            if (__result && !TransportHandler.IgnoreMoveStops.Contains(lineID) && !TransportManager.instance.m_lines.m_buffer[lineID].m_flags.IsFlagSet(TransportLine.Flags.Temporary))
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