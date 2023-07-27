using Dungeon.Data;
using Dungeon.Gameplay;
using Manager.SubManager;
using UnityEngine;

namespace Manager {
    public class ManagerMonitor : MonoBehaviour {
        [Header("LevelManager")]
        public string currentSceneOpen;
        public string currentLevelOpen;

        [Header("DungeonManager")] 
        public int availableDungeons;
        public ScoreCounter scoreCounter;
        
        private void FixedUpdate() {
            currentSceneOpen = LevelManager.currentSceneOpen;
            currentLevelOpen = LevelManager.currentSceneOpen;

            availableDungeons = DungeonList.dungeons.Count;
            scoreCounter = DungeonManager.ScoreCounter;
        }

    }
}
