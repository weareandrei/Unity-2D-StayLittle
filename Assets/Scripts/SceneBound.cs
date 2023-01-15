using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneBound : MonoBehaviour
{

    [SerializeField] public string connectedSceneName;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            LevelManager.moveObjectToAnotherScene(other.gameObject, connectedSceneName);
        }
    }
}
