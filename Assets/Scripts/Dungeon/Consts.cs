using System;
using System.Collections.Generic;
using Global;

namespace Dungeon {
    public static class Consts {

        private static readonly Dictionary<string, object> variables = new Dictionary<string, object>
        {
            { "MaxDungeons", 10 },
            { "ChunkSize", 4 },
            { "DungeonChunkCount", 3 },
            { "RoomSize", 4 },
            { "SpaceBetweenRoomCenters", 20 }, // assuming they are all square
            { "SizeOfRoom_PX", 23.7f }, // Check later if it's specific size units or a relative scale
            { "SpaceBetweenDungeons", 60 }, // Check later if it's specific size units or scale
            
            // Dungeon Contents
            { "MobMultiplier", 10 },
            { "LootMultiplier", 10 }
        };

        public static T Get<T>(string variableName)
        {
            if (variables.ContainsKey(variableName))
            {
                return (T)variables[variableName];
            }

            throw new ArgumentException($"Variable '{variableName}' does not exist in Consts.");
        }

        public static void Set<T>(string variableName, T value)
        {
            if (GlobalVariables.environment == "DEV")
            {
                if (variables.ContainsKey(variableName))
                {
                    variables[variableName] = value;
                    return;
                }

                throw new ArgumentException($"Variable '{variableName}' does not exist in Consts.");
            }
        }
    }
}