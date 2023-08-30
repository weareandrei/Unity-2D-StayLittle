using UnityEngine;
using UnityEngine.UIElements;
using UnitBase = Unit.Base;

namespace UI.Unit {
    public class PlayerStatsUI : BaseStatsUI {
        
        protected float displayedXP;
        private UnitBase.Unit playerUnit;
        
        private void Start() {
            TryToFindPlayer();
        }

        private void FixedUpdate() {
            if (playerUnit == null) {
                TryToFindPlayer();
                return;
            }
            
            UpdateHP();
            UpdateXP();
            UpdateLevel();
        }

        private void TryToFindPlayer() {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj == null) { return; }
            playerUnit = playerObj.GetComponent<UnitBase.Unit>();
        }

        private void UpdateHP() {
            float playerCurrentHP = playerUnit.stats.CurrentHP;
            float playerMaxHP = playerUnit.stats.MaxHP;
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
            float playerCurrentXP = playerUnit.stats.CurrentHP;
            float playerMaxXP = playerUnit.stats.MaxHP;
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
            float playerCurrentLevel = playerUnit.stats.CurrentLevel;

            // Update the text of the percentage label
            Label amountLabel = visualElement.Q<Label>("LevelNumber");
            amountLabel.text = playerCurrentLevel.ToString("F0");
        }

    }
}