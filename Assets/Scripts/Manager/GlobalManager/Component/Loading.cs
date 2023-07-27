using UnityEngine;

namespace Manager {
    
    public static class Loading {
        public delegate void DoBeforeLoading(System.Action InstantiateLoadingScreen);

        public static void Start (DoBeforeLoading doBeforeLoading) {
            if (doBeforeLoading != null) {
                doBeforeLoading.Invoke(StartLoading);
                return;
            }

            StartLoading();
        }
        
        public static void StartLoading() {
            InstantiateLoadingScreen();
            // ...
        }

        private static void InstantiateLoadingScreen() {
            // The path to the LoadingScreen prefab
            string loadingScreenPrefabPath = "DEV/UI/LoadingScreen";

            // Load the prefab from the Resources folder
            GameObject loadingScreenPrefab = Resources.Load<GameObject>(loadingScreenPrefabPath);

            if (loadingScreenPrefab == null)
            {
                Debug.LogError("LoadingScreen prefab not found at path: " + loadingScreenPrefabPath);
                return;
            }

            // Instantiate the prefab at (0, 0) in the world space
            GameObject.Instantiate(loadingScreenPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
    }
}