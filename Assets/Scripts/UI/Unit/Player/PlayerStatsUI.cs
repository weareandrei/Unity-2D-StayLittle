using Dungeon.Gameplay;
using Unit.Player;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Unit {
    public class PlayerStatsUI : BaseStatsUI {
        
        protected float displayedXP;

        private void FixedUpdate() {
            UpdateHP();
            UpdateXP();
            UpdateLevel();
        }

        private void UpdateHP() {
            float playerCurrentHP = thisUnit.stats.CurrentHP;
            float playerMaxHP = thisUnit.stats.MaxHP;
            float hpPercentage = (playerCurrentHP / playerMaxHP) * 100;

            // Update the text of the percentage label
            Label percentLabel = visualElement.Q<Label>("Percent");
            percentLabel.text = hpPercentage.ToString("F0") + "%";

            // Update the width of the progress bar container
            VisualElement progressBarContainer = visualElement.Q("ProgressBarContainer_HP");
            VisualElement progressBar = progressBarContainer.Q("Progress_HP");

            float newWidth = (playerCurrentHP / playerMaxHP) * progressBarContainer.resolvedStyle.width;
            progressBar.style.width = newWidth;
        }

        private void UpdateXP() {
            PlayerUnit playerUnit = thisUnit as PlayerUnit;

            float playerCurrentXP = playerUnit.stats.CurrentXP;
            float playerMaxXP = playerUnit.stats.MaxXP;
            float xpPercentage = (playerCurrentXP / playerMaxXP) * 100;

            // Update the text of the percentage label
            Label amountLabel = visualElement.Q<Label>("AmountXP");
            amountLabel.text = playerCurrentXP.ToString("F0") + " / " + playerMaxXP.ToString("F0");

            // Update the width of the progress bar container
            VisualElement progressBarContainer = visualElement.Q("ProgressBarContainer_XP");
            VisualElement progressBar = progressBarContainer.Q("Progress_XP");

            float newWidth = (playerCurrentXP / playerMaxXP) * progressBarContainer.resolvedStyle.width;
            progressBar.style.width = newWidth;
        }

        private void UpdateLevel() {
            float playerCurrentLevel = thisUnit.stats.CurrentLevel;

            // Update the text of the percentage label
            Label amountLabel = visualElement.Q<Label>("LevelNumber");
            amountLabel.text = playerCurrentLevel.ToString("F0");
        }

    }
}