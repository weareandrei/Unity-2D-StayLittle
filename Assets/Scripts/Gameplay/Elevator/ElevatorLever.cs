using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Interaction {
    public class ElevatorLever : MonoBehaviour {
        [SerializeField] private float activationRadius = 2f;
        [SerializeField] private KeyCode activationKey = KeyCode.E;
        [SerializeField] private GameObject elevator;
        [SerializeField] private bool activated = false;
 
        private bool playerInRange = false;

        /* -----   MonoBehaviour   -----*/
        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                playerInRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.CompareTag("Player")) {
                playerInRange = false;
            }
        }
        
        private void Update() {
            if (playerInRange && Input.GetKeyDown(activationKey)) {
                ActivateLever();
            }
        }

        /* -----   !MonoBehaviour   -----*/


        private void ActivateLever() {
            // Perform the lever activation logic here
            this.activated = true;
            elevator.GetComponent<ElevatorController>().Activate();
        }
    }
}