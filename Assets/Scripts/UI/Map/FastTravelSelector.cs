using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using Dungeon.Data;
using Interaction;
using Manager;
using Manager.SubManager;
using UnityEngine.UIElements;
using UI.Base;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI.Map {
    
    public class FastTravelSelector : UIElement {
        
        public void Display() {
            visualElement.style.display = DisplayStyle.Flex;
            VisualElement optionsContainer = visualElement.Q<VisualElement>("Options");
            optionsContainer.Clear();

            List<FastTravelOption> optionsAvailable = GetFastTravelOptions();

            for (int i = 0; i < optionsAvailable.Count; i++) {
                CreateButton(optionsAvailable[i]);
            }
        }
    
        private List<FastTravelOption> GetFastTravelOptions() {
            List<FastTravelOption> fastTravelOptions = new List<FastTravelOption>();

            if (LevelManager.currentSceneOpen == "Dungeon") {
                fastTravelOptions.Add(new FastTravelOption("Home", "Home"));
            }
            fastTravelOptions.Add(new FastTravelOption("RandomDungeon", "RandomDungeon"));

            fastTravelOptions.AddRange(
                DungeonList.availableDungeons.Select(
                    dungeonData => new FastTravelOption("RegularDungeon", dungeonData.id))
                );

            return fastTravelOptions;
        }

        private void CreateButton(FastTravelOption fastTravelOption) {
            
            Button button = new Button();
            button.text = fastTravelOption.destinationId;
            button.clicked += () => SelectFastTravel(fastTravelOption);

            VisualElement optionsContainer = visualElement.Q<VisualElement>("Options");
            optionsContainer.Add(button);
        }

        public void SelectFastTravel(FastTravelOption fastTravelOption) {
            UIManager.Instance.HideNonPersistentUI();
            ElevatorController elevatorController = GameObject.FindObjectOfType<ElevatorController>();
            elevatorController.moveParams = PrepareMovementParams(fastTravelOption);
            elevatorController.ChangeState(ElevatorState.StartMoving);
        }
        
        private ElevatorMovementParameters PrepareMovementParams(FastTravelOption fastTravelOption) {
            if (fastTravelOption.destinationType == "RegularDungeon") {
                fastTravelOption.destinationType = "AvailableDungeon";
            }
            // todo: above code is bad ^
            
            ElevatorMovementParameters moveParams = default;
            
            switch (fastTravelOption.destinationType) {
                case "Home":
                    moveParams.direction = 1;
                    moveParams.goToDungeon = null;
                    moveParams.startInstantly = false;
                    break;
                case "RandomDungeon":
                    string dungeonId = DungeonManager.ChooseRandomDungeon();
                    moveParams.targetCoordinateY = GetTargetCoordinateY(dungeonId);
                    moveParams.direction = CalculateMovementDirection(moveParams.targetCoordinateY);
                    moveParams.goToDungeon = dungeonId;
                    moveParams.startInstantly = false;
                    break;
                case "AvailableDungeon":
                    moveParams.goToDungeon = fastTravelOption.destinationId;
                    moveParams.targetCoordinateY = GetTargetCoordinateY(fastTravelOption.destinationId);
                    moveParams.direction = CalculateMovementDirection(moveParams.targetCoordinateY);
                    moveParams.startInstantly = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return moveParams;
        }

        private int CalculateMovementDirection(float destinationCoordinateY) {
            if (transform.position.y > destinationCoordinateY) {
                return -1;
            }

            return 1;
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
}

[Serializable]
public struct FastTravelOption {
    public string destinationType;
    public string destinationId;

    public FastTravelOption(string destinationType) {
        this.destinationType = destinationType;
        this.destinationId = null;
    }
    
    public FastTravelOption(string destinationType, string destinationId) {
        this.destinationType = destinationType;
        this.destinationId = destinationId;
    }
}

[Serializable]
public enum FastTravelDestinations {
    Home = 0,
    RandomDungeon = 1,
    AvailableDungeon = 2
}