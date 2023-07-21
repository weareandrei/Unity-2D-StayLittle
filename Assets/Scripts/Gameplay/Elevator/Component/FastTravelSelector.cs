using System;
using System.Collections.Generic;
using System.Linq;
using Dungeon;
using Dungeon.Data;
using TMPro;
using Manager.SubManager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Interaction.Component {
    
    public class FastTravelSelector : MonoBehaviour {
        [SerializeField] private ElevatorController elevatorController;
        
        // public GameObject buttonPrefab; // Prefab for the individual button
        // public GameObject windowPrefab; // The prefab for the window background

        public int buttonWidth = 100;
        public int buttonHeight = 40;
        public int buttonSpacing = 10;

        public int minimumContainerHeight = 300;

        private RectTransform containerRect;

        // Function to open the small size window in the middle of the game screen
        public void Open() {
            List<FastTravelOption> optionsAvailable = GetFastTravelOptions(); // Array of available options for fast travel

            for (int i = 0; i < optionsAvailable.Count; i++) {
                CreateButton(optionsAvailable[i], i * (buttonHeight + buttonSpacing) + buttonSpacing);
            }
        }
    
        private List<FastTravelOption> GetFastTravelOptions() {
            List<FastTravelOption> fastTravelOptions = new List<FastTravelOption>();
            
            fastTravelOptions.Add(new FastTravelOption("Home", "Home"));
            fastTravelOptions.Add(new FastTravelOption("RandomDungeon", "RandomDungeon"));

            fastTravelOptions.AddRange(
                DungeonList.availableDungeons.Select(
                    dungeonData => new FastTravelOption("RegularDungeon", dungeonData.id))
                );

            return fastTravelOptions;
        }

        private void CreateButton(FastTravelOption fastTravelOption, int yOffset) {
            GameObject buttonObj = new GameObject("Button");
            buttonObj.transform.SetParent(transform);

            RectTransform buttonRect = buttonObj.AddComponent<RectTransform>();
            buttonRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
            buttonRect.anchoredPosition = new Vector2(0, -yOffset);

            Image buttonBackground = buttonObj.AddComponent<Image>();
            buttonBackground.color = Color.black;

            Button buttonComponent = buttonObj.AddComponent<Button>();

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(buttonObj.transform);

            RectTransform textRect = textObj.AddComponent<RectTransform>();
            textRect.sizeDelta = new Vector2(buttonWidth, buttonHeight);
    
            // Set the pivot of the text to the center (0.5, 0.5)
            textRect.pivot = new Vector2(0.5f, 0.5f);

            // Set the anchor and position of the text to center within the button
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.anchoredPosition = Vector2.zero;

            // Replace TMP_Text with TextMeshProUGUI
            TextMeshProUGUI buttonTextComponent = textObj.AddComponent<TextMeshProUGUI>();
            buttonTextComponent.text = fastTravelOption.destinationId;
            buttonTextComponent.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Arial SDF"); // Change the font path according to your imported TMP fonts
            buttonTextComponent.fontSize = 20;
            buttonTextComponent.alignment = TextAlignmentOptions.Center; // TMP uses different alignment options
            buttonTextComponent.color = Color.white;

            buttonComponent.onClick.AddListener(() => SelectFastTravel(fastTravelOption));
        }

        public void SelectFastTravel(FastTravelOption fastTravelOption) {
            elevatorController.moveParams = PrepareMovementParams(fastTravelOption);
            elevatorController.ChangeState(ElevatorState.StartMoving);
            this.gameObject.SetActive(false);
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