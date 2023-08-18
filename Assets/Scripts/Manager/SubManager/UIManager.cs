using System;
using Dungeon.Gameplay;
using UnityEngine;
using UnityEngine.UIElements;
using Util;
using UI.Dungeon;
using UI.Map;
using UI.Player;

namespace Manager.SubManager {
    public class UIManager : PersistentSingleton<UIManager> {

        private GameObject scoreCounterDisplay;
        private GameObject fastTravel;

        private GameObject playerUI;
        // private GameObject loadingScreen;

        [SerializeField] private bool UIVisible;

        public void Start() {
            scoreCounterDisplay = transform.Find("ScoreCounterDisplay").gameObject;
            fastTravel = transform.Find("FastTravelControl").gameObject;
            playerUI = transform.Find("PlayerUI").gameObject;
            // loadingScreen = transform.Find("LoadingScreen").gameObject.GetComponent<UIDocument>().rootVisualElement;

            HideNonPersistentUI();
        }
        
        public void HideNonPersistentUI() {
            scoreCounterDisplay.GetComponent<ScoreCounterDisplay>().Hide();
            fastTravel.GetComponent<FastTravelSelector>().Hide();
            playerUI.GetComponent<PlayerUI>().Hide();
            // loadingScreen.GetComponent<ScoreCounterDisplay>().Hide();

            UIVisible = false;
        }

        public void ShowRequiredLevelUI() {
            playerUI.GetComponent<PlayerUI>().Display();
        }
        
        // ScoreDisplay should be in the side part of the screen, it should be small, not a central large piece of UI!.
        // Like when a task or message appears.
        public void OpenScoreDisplay(ScoreCounter scoreCounter) {
            scoreCounterDisplay.GetComponent<ScoreCounterDisplay>().Display(scoreCounter);
            scoreCounterDisplay.GetComponent<ScoreCounterDisplay>().AutoHideTimer(5f);
            UIVisible = true;
        }
        
        public void OpenFastTravel() {
            fastTravel.GetComponent<FastTravelSelector>().Display();
            UIVisible = true;
        }

        public void OpenPlayerUI() {
            playerUI.GetComponent<FastTravelSelector>().Display();
            UIVisible = true;
        }
        
        // public void OpenLoadingScreen() {
        //     scoreCounter.style.display = DisplayStyle.Flex;
        //     UIVisible = true;
        // }
        
    }
}