using System;
using System.Collections;
using Interaction.Component;
using Manager.SubManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interaction {
    public class ElevatorController : MonoBehaviour {
        
        private SimpleMovementController simpleMovementController;
        public GameObject movingPlatform;
        private GameObject fastTravelSelector;
        
        private IEnumerator movementCoroutine;

        private float initialY;
        
        [SerializeField] public ElevatorMovementParameters moveParams;
        
        
        [SerializeField] public float maxSpeed = 15f;
        
        [SerializeField] private float acceleration = 2f;
        [SerializeField] private float stoppingSpeed = 6f;
        
        [SerializeField] private float currentSpeed = 0f;
        [SerializeField] private float minimumDistanceToStop = 0.4f;

        [SerializeField] public ElevatorState State { get; private set; }

        public static event Action<ElevatorState> OnBeforeStateChanged;
        public static event Action<ElevatorState> OnAfterStateChanged;
            
        private void Awake() {
            simpleMovementController = GetComponent<SimpleMovementController>();
            fastTravelSelector = transform.Find("FastTravelSelector").gameObject;
            initialY = transform.position.y;
            // fastTravelSelector = transform.Find("FastTravelSelector").gameObject.GetComponent<FastTravelSelector>();
        }

        private void Start() => ChangeState(ElevatorState.Idle);

        public void ChangeState(ElevatorState newState) {
            OnBeforeStateChanged?.Invoke(newState);

            State = newState;
            switch (newState) {
                case ElevatorState.Idle:
                    break;
                case ElevatorState.Launching:
                    HandleElevatorLaunch();
                    break;
                case ElevatorState.SelectingFastTravelOptions:
                    HandleOptionSelection();
                    break;
                case ElevatorState.StartMoving:
                    HandleElevatorStartMoving();
                    break;
                case ElevatorState.Moving:
                    break;
                case ElevatorState.ContinueMoving:
                    this.moveParams.startInstantly = true;
                    HandleElevatorStartMoving();
                    break;
                case ElevatorState.Stopping:
                    BeginStopping();
                    break;
                case ElevatorState.FinishMoving:
                    // HandleFinishMoving();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            OnAfterStateChanged?.Invoke(newState);
        }

        public void Activate() {
            if (State != ElevatorState.Idle) {
                return;
            }
            ChangeState(ElevatorState.Launching);
        }

        private void HandleElevatorLaunch() {
            StartCoroutine(LaunchCoroutine());
            ChangeState(ElevatorState.SelectingFastTravelOptions);
        }
        private void HandleOptionSelection() {
            fastTravelSelector.SetActive(true);
            fastTravelSelector.GetComponent<FastTravelSelector>().Open();
            // ChangeState(ElevatorState.StartMovingUp);
        }
        
        private void HandleElevatorStartMoving() {
            if (!moveParams.startInstantly) {
                DungeonManager.DungeonSelected(moveParams.goToDungeon);
            }

            movementCoroutine = MoveElevator();
            StartCoroutine(movementCoroutine);
            
            ChangeState(ElevatorState.Moving);
        }
        
        public void ContinueMoving (ElevatorMovementParameters newMoveParams) {
            this.moveParams = newMoveParams;
            ChangeState(ElevatorState.ContinueMoving);
        }
        
        private IEnumerator LaunchCoroutine() {
            yield return null;
        }
        
        private IEnumerator MoveElevator() {
            StartSimpleMovement();
            
            while (MustKeepMoving()) {
                if (moveParams.startInstantly) {
                    currentSpeed = maxSpeed;
                }
                
                if (currentSpeed < maxSpeed) {
                    currentSpeed += acceleration * Time.fixedDeltaTime;
                }
                
                float moveDistance = currentSpeed * Time.fixedDeltaTime;
                Vector3 moveDir = moveParams.direction == -1 ? Vector3.down : Vector3.up;
                movingPlatform.transform.Translate(moveDir * moveDistance);

                // yield return new WaitForSeconds(0.1f);
                yield return new WaitForFixedUpdate();
            }
            
            ChangeState(ElevatorState.Stopping);

            yield return null;
        }

        private bool MustKeepMoving() {
            if (SceneManager.GetActiveScene().name == "Dungeon") {
                if (moveParams.direction == -1 && GetDistanceToTarget() > minimumDistanceToStop) {
                    return true;
                }
                if (moveParams.direction == 1) {
                    return true;
                }
            }
            
            if (SceneManager.GetActiveScene().name != "Dungeon") {
                if (moveParams.direction == 1 && GetDistanceToTarget() > minimumDistanceToStop) {
                    return true;
                }
                if (moveParams.direction == -1) {
                    return true;
                }
            }

            return false;
        }

        private void StartSimpleMovement() {
            simpleMovementController.ActionInvoked = true;
        }
        
        private void BeginStopping() {
            movementCoroutine = StopElevator();
            StartCoroutine(movementCoroutine);
        }

        private IEnumerator StopElevator() {
            ChangeState(ElevatorState.Idle);
            // transform.Translate(moveParams.direction == 1 ? Vector3.down : Vector3.up * moveDistance);
            yield return null;
        }

        private float GetDistanceToTarget() {
            if (moveParams.direction == 1) {
                
            }
            return Mathf.Abs(moveParams.targetCoordinateY - movingPlatform.transform.position.y);
        }
        

    }
}

[Serializable]
public enum ElevatorState {
    Idle = 0,
    Launching = 1,
    SelectingFastTravelOptions = 2,
    StartMoving = 3,
    Moving = 4,
    ContinueMoving = 5,
    Stopping = 6,
    ShowingResults = 7,
    FinishMoving = 8
}

[Serializable]
public struct ElevatorMovementParameters {
    [SerializeField] public int direction;
    [SerializeField] public float targetCoordinateY;
    [SerializeField] public string goToDungeon;
    [SerializeField] public bool startInstantly;
}