using System;
using System.Collections;
using Interaction.Component;
using Manager.SubManager;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Interaction {
    public class ElevatorController : MonoBehaviour {
        
        private SimpleMovementController simpleMovementController;
        private GameObject movingPlatform;
        private GameObject fastTravelSelector;
        
        private IEnumerator movementCoroutine;
        
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
            
            DungeonManager.DungeonSelected(moveParams.goToDungeon);
            
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
            
            while (SceneManager.GetActiveScene().name != "Dungeon" || GetDistanceToTarget() > minimumDistanceToStop) {
                if (moveParams.startInstantly) {
                    currentSpeed = maxSpeed;
                }
                
                if (currentSpeed < maxSpeed) {
                    currentSpeed += acceleration * Time.fixedDeltaTime;
                }
                
                float moveDistance = currentSpeed * Time.fixedDeltaTime;
                Vector3 moveDir = moveParams.direction == -1 ? Vector3.down : Vector3.up;
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

[Serializable]
public enum ElevatorState {
    Idle = 0,
    Launching = 1,
    SelectingFastTravelOptions = 2,
    StartMoving = 3,
    Moving = 4,
    ContinueMoving = 5,
    ShowingResults = 6,
    FinishMoving = 7
}

[Serializable]
public struct ElevatorMovementParameters {
    [SerializeField] public int direction;
    [SerializeField] public float targetCoordinateY;
    [SerializeField] public string goToDungeon;
    [SerializeField] public bool startInstantly;
}