using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float elevatorMovementSpeed = 1f;
    [SerializeField] private bool playerEntered = false;
    [SerializeField] private bool movingDown = false;

    [SerializeField] private GameObject movingPart;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerEntered = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            playerEntered = false;
        }
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.E)) {
            if (playerEntered) {
                movingDown = true;
            }
        }

        if (movingDown) {
            moveDown();
        }
    }

    private void moveDown() {
        movingPart.transform.Translate(elevatorMovementSpeed * (-1f) * Vector3.up * Time.deltaTime, Space.World);
    }

}
