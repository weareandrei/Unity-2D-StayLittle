using UnityEditor;
using System.Threading.Tasks;
using UnityEngine;

namespace UI {
    public static class LoadingManager {
        
        public delegate void BeforeLoadingDelegate();
        
        public static BeforeLoadingDelegate BeforeLoading;

        private static GameObject loadingPrefab = null;
        private static GameObject loadingScreenInstance;
        
        public static void StartLoading() {
            string prefabPath = "DEV/UI/LoadingScreen";
            GameObject loadingPrefab = Resources.Load<GameObject>(prefabPath);
            loadingScreenInstance = GameObject.Instantiate(loadingPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            loadingPrefab.SetActive(true);
        }

        public static void StopLoading() {
            loadingScreenInstance.SetActive(false);
        }

        // public static void BeginLoading() {
        //     if (BeforeLoading != null) {
        //         BeforeLoading().then(Loading());
        //     }
        //
        //     Loading();
        // }
        
        // public static async Task BeginLoadingAsync() {
        //     if (BeforeLoading != null) {
        //         await BeforeLoading();
        //     }
        //     
        //     await LoadContentAsync();
        // }
        //
        //
        // private static Task Loading() {
        //     return Task.CompletedTask;
        //     // Do the loading
        // }
    }
}