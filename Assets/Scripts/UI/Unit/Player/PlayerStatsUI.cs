using Dungeon.Gameplay;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Unit {
    public class PlayerStatsUI : BaseStatsUI {
        
        private void FixedUpdate() {
            UpdateHP();
            UpdateXP();
            UpdateLevel();
        }

        private void UpdateHP() {
            float playerCurrentHP = thisUnit.GetHP();
            float playerMaxHP = thisUnit.GetMaxHP();
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

        private void UpdateXP() {
            float playerCurrentHP = thisUnit.GetHP();
            float playerMaxHP = thisUnit.GetMaxHP();
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

        private void UpdateLevel() {
            
        }

        public void Display(ScoreCounter scoreCounter) {
            // Show the UI element if it's hidden
            visualElement.style.display = DisplayStyle.Flex;
        }

    }
}