using UnityEditor;
using System.Threading.Tasks;
using UnityEngine;

namespace UI {
    public static class LoadingManager {
        
        public delegate void BeforeLoadingDelegate();
        
        public static BeforeLoadingDelegate BeforeLoading;
        private static GameObject _loadingScreenInstance = null;
        
        public static void StartLoading() {
            if (_loadingScreenInstance == null) {
                string prefabPath = "DEV/UI/LoadingScreen";
                GameObject loadingPrefab = Resources.Load<GameObject>(prefabPath);
                _loadingScreenInstance = GameObject.Instantiate(loadingPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
            }
            
            _loadingScreenInstance.SetActive(true);
        }

        public static void StopLoading() {
            _loadingScreenInstance.SetActive(false);
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