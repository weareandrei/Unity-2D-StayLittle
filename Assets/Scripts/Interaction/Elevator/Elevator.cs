using System;
using Dungeon;
using Dungeon.Data;
using Manager.SubManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Interaction
{
    public class Elevator : MonoBehaviour {
        private ElevatorController elevatorController;
        
        public static event Action DungeonManagerEvent;

        private void Awake() {
            elevatorController = GetComponent<ElevatorController>();
        }

        public void Activate() {
            ElevatorMovementParameters moveParams = PrepareMovementParams();
            if (moveParams.direction == -1 ) {
                if (PlayerCompletedDungeon()) {
                    DungeonManager.ShowScore().then(elevatorController.Move(moveParams));
                }
                else {
                    NotAllowedToGoUp();
                }
            }
            else {
                elevatorController.Move(moveParams);
            }
        }

        private bool PlayerCompletedDungeon() {
            return DungeonManager.PlayerLeavesDungeon();
        }

        private ElevatorMovementParameters PrepareMovementParams() {
            ElevatorMovementParameters moveParams;
            if (SceneManager.GetActiveScene().name != "Dungeon") {
                moveParams.direction = 1;
            }
            else {
                // If we need to go up
                moveParams.direction = -1;
            }
            moveParams.goToDungeon = ChooseRandomDungeon();
            moveParams.targetCoordinateY = GetTargetCoordinateY(moveParams.goToDungeon);
            moveParams.startInstantly = false;

            return moveParams;
        }
        
        public static string ChooseRandomDungeon() {
            foreach (DungeonData dungeonData in DungeonList._dungeons) {
                if (dungeonData.typeOfDungeon == DungeonType.regularDungeon) {
                    return dungeonData.id;
                }
            }

            throw new Exception();
        }

        public float GetTargetCoordinateY(string id) {
            DungeonData selectedDungeon = DungeonList.GetDungeonDataByID(id);
            // Selecting a random entrance to go into
            int numberOfEntrances = selectedDungeon.entrances.Count;
            int randomNumber = Random.Range(0, numberOfEntrances-1);
            float entranceCoordFromDungeonOrigin =
                selectedDungeon.entrances[randomNumber].y * Consts.Get<float>("SizeOfRoom_PX");
            return selectedDungeon.coordinates.y + entranceCoordFromDungeonOrigin;
        }

    }

    [Serializable]
    public struct ElevatorMovementParameters {
        [SerializeField] public int direction;
        [SerializeField] public float targetCoordinateY;
        [SerializeField] public string goToDungeon;
        [SerializeField] public bool startInstantly;
    }
}