using System.Collections.Generic;

namespace Global {
    
    public static class GlobalVariables
    {
        public static string environment = "PROD"; // PROD or DEV

        private static Dictionary<string, string> resourcesDirectories = new Dictionary<string, string>() {
            { "PROD", "PROD/" },
            { "DEV", "DEV/" }
        }; public static string ResourcesDirectory => resourcesDirectories[environment];
    }
    
}

