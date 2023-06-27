using Manager;
using UnityEngine;

namespace Interaction {
    
    public class SceneBound : MonoBehaviour {

        [SerializeField] public string connectedSceneName;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                LevelManager.MoveObjectToAnotherScene(other.gameObject, connectedSceneName);
            }
        }
    }
    
}
