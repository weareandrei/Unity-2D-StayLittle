using System;
using System.Collections;
using Dungeon.Gameplay;
using Manager;
using UnityEngine;

namespace UI.Dungeon {
    public class ScoreScreen : MonoBehaviour {
        public ScoreCounter scoreCounter;

        private void Start() {
            StartCoroutine(WaitAndFinish());
        }

        private IEnumerator WaitAndFinish() {
            yield return new WaitForSeconds(3f);
            Debug.Log("Waited for 3 seconds. Coroutine finished!");
            GameManager.ChangeState(GameState.Playing);
            Destroy(gameObject);
        }

    }
}