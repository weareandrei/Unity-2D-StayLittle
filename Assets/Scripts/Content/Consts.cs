using System;
using System.Collections.Generic;
using Global;

namespace Content {
    public static class Consts {

        private static readonly Dictionary<string, object> variables = new Dictionary<string, object>
        {
            // This is all PER ROOM :
            { "MaxEnvironmental", 2 },
            { "MaxEntity", 2 },
            { "MaxCollectibleWorld", 3 },
            { "MaxCollectibleLoot", 3 }
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