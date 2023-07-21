using Dungeon.Gameplay;
using Manager;
using UnityEngine;

namespace UI.Dungeon {
    public static class ScoreDisplay {

        public static void InstantiateScoreScreen(ScoreCounter scoreCounter) {
            GameManager.ChangeState(GameState.PauseOnUI);
            
            string prefabPath = "DEV/UI/Dungeon/ScoreScreen";
            GameObject scoreScreenPrefab = Resources.Load<GameObject>(prefabPath);
            scoreScreenPrefab.GetComponent<ScoreScreen>().scoreCounter = scoreCounter;
            // Pass in the scoreCounter to scorePrefab
            GameObject.Instantiate(scoreScreenPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        }
        
    }
}