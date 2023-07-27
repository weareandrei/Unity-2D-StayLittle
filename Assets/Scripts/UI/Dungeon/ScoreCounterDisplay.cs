using System.Collections;
using Dungeon.Gameplay;
using UI.Base;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.Dungeon {
    public class ScoreCounterDisplay : UIElement {

        public void Display(ScoreCounter scoreCounter) {
            visualElement.Q<Label>("ScorePoints").text = scoreCounter.pointsEarned.ToString();
            visualElement.Q<Label>("MobsKilled").text = scoreCounter.mobsKilled.ToString();
            visualElement.style.display = DisplayStyle.Flex;
        }

        public void AutoHideTimer(float seconds) {
            StartCoroutine(HideTimer(seconds));
        }
        
        IEnumerator HideTimer(float seconds) {
            yield return new WaitForSeconds(seconds);
            Hide();
        }

    }
}