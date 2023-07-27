using System;
using UnityEngine;

namespace Interaction {
    public class SimpleMovement : MonoBehaviour {
        public bool rotation;
        public bool linearMovement;

        public int direction;
        public float moveSpeed;
        public bool repeatable;

        private bool isMoving;
        private Quaternion initialRotation;
        private Vector3 initialPosition;

        [SerializeField] private SimpleMovementController controller;

        private void Start() {
            // Store the initial rotation and position of the GameObject
            initialRotation = transform.rotation;
            initialPosition = transform.position;
        }

        private void FixedUpdate() {
            if (isMoving) {
                PerformAction();
            }
        }

        private void OnEnable() {
            // Subscribe to the moveActions event
            controller.moveActions += StartMovement;
        }

        private void OnDisable() {
            // Unsubscribe from the moveActions event
            controller.moveActions -= StartMovement;
        }

        private void StartMovement() {
            isMoving = true;
        }

        private void StopMovement() {
            isMoving = false;
        }

        private void PerformAction() {
            // Perform the required action considering the parameters given
            // This could be moving or rotating
            if (rotation) {
                Rotate();
            }

            if (linearMovement) {
                Move();
            }
        }

        private void Rotate() {
            // Rotate the GameObject based on the specified direction
            transform.rotation *= Quaternion.Euler(0f, 0f, direction);
        }

        private void Move() {
            // Move the GameObject based on the specified move speed and direction
            float distance = moveSpeed * Time.fixedDeltaTime;
            transform.Translate(transform.up * direction * distance, Space.World);
        }

    }
}
