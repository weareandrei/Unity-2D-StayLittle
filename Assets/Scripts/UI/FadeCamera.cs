using UnityEngine;

namespace UI {
    public static class FadeCamera {
        public static void StartFading() {
            InstantiateFadingScreen();
            // ...
        }

        private static void InstantiateFadingScreen() {
            // The path to the LoadingScreen prefab
            string fadingScreenPrefabPath = "DEV/UI/FadingScreen";

            // Load the prefab from the Resources folder
            GameObject fadingScreenPrefab = Resources.Load<GameObject>(fadingScreenPrefabPath);

            if (fadingScreenPrefab == null)
            {
                Debug.LogError("fadingScreen prefab not found at path: " + fadingScreenPrefabPath);
                return;
            }

            // Instantiate the prefab at (0, 0) in the world space
            GameObject.Instantiate(fadingScreenPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }
}