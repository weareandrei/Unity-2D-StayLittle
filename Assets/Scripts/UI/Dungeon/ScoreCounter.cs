using Dungeon.Gameplay;
using UnityEngine;

namespace UI.Dungeon {
    public static class ScoreDisplay {
        private static string prefabPath = "DEV/UI/ScoreScreen";
        // private GameObject 
        
        public static void InstantiateScoreScreen(ScoreCounter scoreCounter) {
            // The path to the LoadingScreen prefab
            string prefabPath = "DEV/UI/LoadingScreen";

            // Load the prefab from the Resources folder
            GameObject loadingPrefab = Resources.Load<GameObject>(prefabPath);

            if (loadingPrefab == null)
            {
                Debug.LogError("LoadingScreen prefab not found at path: " + prefabPath);
                return;
            }

            // Instantiate the prefab at (0, 0) in the world space
            GameObject.Instantiate(loadingPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
        
    }
}