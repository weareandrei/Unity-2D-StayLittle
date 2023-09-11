using UnityEngine;
using System.Collections.Generic;
using Unit.AI.Navigation;
using UnityEditor;

namespace Unit.AI {
    public class ActionPoint : NavPoint {
        private List<GameObject> collidingObjects = new List<GameObject>();
        private bool isSendingSignal = false;

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.GetComponentInChildren<Pathfinder>() != null) {
                collidingObjects.Add(other.gameObject);

                // If we're not already sending signals, start sending them
                if (!isSendingSignal) {
                    isSendingSignal = true;
                    InvokeRepeating("SendActionSignal", 0f, 1f);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (other.GetComponentInChildren<Pathfinder>() != null) {
                collidingObjects.Remove(other.gameObject);

                // If there are no more colliding objects, stop sending signals
                if (collidingObjects.Count == 0) {
                    isSendingSignal = false;
                    CancelInvoke("SendActionSignal");
                }
            }
        }

        private void SendActionSignal() {
            foreach (var obj in collidingObjects) {
                Pathfinder pathfinder = obj.GetComponentInChildren<Pathfinder>();
                if (pathfinder != null) {
                    pathfinder.ActionPointDetected();
                }
            }
        }

        private void OnDrawGizmos() {
            if (gizmoColor == default) {
                gizmoColor = Random.ColorHSV(0.5f, 0.5f, 0.2f, 0.2f, 1f, 1f, 0.5f, 0.5f); // 20% transparent
            }

            Handles.color = gizmoColor;
            Gizmos.DrawWireCube(transform.position, new Vector3(0.3f, 0.3f));
        }
    }
}