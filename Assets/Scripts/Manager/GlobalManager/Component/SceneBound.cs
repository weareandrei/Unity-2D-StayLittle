using System.Numerics;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interaction {
    
    public class SceneBound : MonoBehaviour {

        [SerializeField] public string connectedSceneName;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                // string currentScene = SceneManager.GetActiveScene().name;
                LevelManager.LoadLevelBasedOnScene(connectedSceneName);
                // LevelManager.MoveObjectToAnotherScene(other.gameObject, currentScene);
            }
        }
    }
    
}
