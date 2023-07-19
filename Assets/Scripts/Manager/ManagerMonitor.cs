using Dungeon.Data;
using Manager.SubManager;
using UnityEngine;

namespace Manager {
    public class ManagerMonitor : MonoBehaviour {
        [Header("LevelManager")]
        public string currentSceneOpen;
        public string currentLevelOpen;

        [Header("DungeonManager")] 
        public int availableDungeons;
        
        private void FixedUpdate() {
            currentSceneOpen = LevelManager.currentSceneOpen;
            currentLevelOpen = LevelManager.currentLevelOpen;

            availableDungeons = DungeonList.dungeons.Count;
        }

    }
}
