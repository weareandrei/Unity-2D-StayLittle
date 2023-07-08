using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interaction {
    public class ElevatorController : MonoBehaviour{
        private SimpleMovementController simpleMovementController;
        private GameObject movingPlatform;
        
        private IEnumerator movementCoroutine;
        
        [SerializeField] public ElevatorMovementParameters moveParams;
        
        
        [SerializeField] public float maxSpeed = 15f;
        
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float stoppingSpeed = 6f;
        
        [SerializeField] private float currentSpeed = 0f;
        [SerializeField] private float minimumDistanceToStop = 0.4f;
        
        private void Awake() {
            simpleMovementController = GetComponent<SimpleMovementController>();
        }
        
        public void Move (ElevatorMovementParameters moveParams) {
            this.moveParams = moveParams;
            movementCoroutine = MoveElevator();
            StartCoroutine(movementCoroutine);
        }
        
        private IEnumerator MoveElevator() {
            StartSimpleMovement();
            
            while (SceneManager.GetActiveScene().name != "Dungeon" || GetDistanceToTarget() > minimumDistanceToStop) {
                if (moveParams.startInstantly) {
                    currentSpeed = maxSpeed;
                }
                
                if (currentSpeed < maxSpeed) {
                    currentSpeed += acceleration * Time.fixedDeltaTime;
                }
                
                float moveDistance = currentSpeed * Time.fixedDeltaTime;
                Vector3 moveDir = moveParams.direction == 1 ? Vector3.down : Vector3.up;
                transform.Translate(moveDir * moveDistance);

                // yield return new WaitForSeconds(0.1f);
                yield return new WaitForFixedUpdate();
            }
            
            BeginStopping();

            yield return null;
        }

        private void StartSimpleMovement() {
            simpleMovementController.ActionInvoked = true;
        }
        
        private void BeginStopping() {
            movementCoroutine = StopElevator();
            StartCoroutine(movementCoroutine);
        }

        private IEnumerator StopElevator() {
            // transform.Translate(moveParams.direction == 1 ? Vector3.down : Vector3.up * moveDistance);
            yield return null;
        }

        private float GetDistanceToTarget() {
            return Mathf.Abs(moveParams.targetCoordinateY - gameObject.transform.position.y);
        }
        

    }
}