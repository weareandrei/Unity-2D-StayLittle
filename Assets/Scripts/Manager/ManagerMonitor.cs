using UnityEngine;

namespace Manager {
    public class ManagerMonitor : MonoBehaviour {
        [Header("LevelManager")]
        public string currentSceneOpen;
        public string currentLevelOpen;

        private void FixedUpdate() {
            currentSceneOpen = LevelManager.currentSceneOpen;
            currentLevelOpen = LevelManager.currentLevelOpen;
        }

    }
}
