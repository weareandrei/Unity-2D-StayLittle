using System;
using System.Collections;
using Dungeon.Gameplay;
using UI.Base;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Player {
    public class PlayerUI : UIElement {
        
        private void FixedUpdate() {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            Unit.Base.Unit playerUnit = player.GetComponent<Unit.Base.Unit>();

            float playerCurrentHP = playerUnit.GetHP();
            float playerMaxHP = playerUnit.GetMaxHP();
            float hpPercentage = (playerCurrentHP / playerMaxHP) * 100;

            // Update the text of the percentage label
            Label percentLabel = visualElement.Q<Label>("Percent");
            percentLabel.text = hpPercentage.ToString("F0") + "%";

            // Update the width of the progress bar container
            VisualElement progressBarContainer = visualElement.Q("ProgressBarContainer");
            VisualElement progressBar = progressBarContainer.Q("Progress");

            float newWidth = (playerCurrentHP / playerMaxHP) * progressBarContainer.resolvedStyle.width;
            progressBar.style.width = newWidth;
        }

        public void Display(ScoreCounter scoreCounter) {
            // Show the UI element if it's hidden
            visualElement.style.display = DisplayStyle.Flex;
        }

    }
}