using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unit.AI {
    public class VisionComponent : MonoBehaviour {
        [SerializeField]
        private Collider visionCollider;

        private List<GameObject> UnregisteredObjects = new List<GameObject>();
        private List<GameObject> RegisteredObjects = new List<GameObject>();
        private List<GameObject> ObjectsToRemove = new List<GameObject>();

        // Events to handle when object enters and exits
        private List<Action<GameObject>> OnObjectEnter = new List<Action<GameObject>>();
        private List<Action<GameObject>> OnObjectExit = new List<Action<GameObject>>();

        private void OnTriggerEnter(Collider other) {
            if (!RegisteredObjects.Contains(other.gameObject)) {
                UnregisteredObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (RegisteredObjects.Contains(other.gameObject)) {
                ObjectsToRemove.Add(other.gameObject);
            }
        }
        
        public void DetectObjectsAround() {
            foreach (GameObject obj in UnregisteredObjects) {
                if (RegisteredObjects.Contains(obj)) {
                    UnregisteredObjects.Remove(obj);
                    continue;
                }

                RegisteredObjects.Add(obj);
                InvokeEnterEvents(obj);
            }
            UnregisteredObjects.Clear();

            foreach (GameObject obj in ObjectsToRemove) {
                if (!RegisteredObjects.Contains(obj)) {
                    ObjectsToRemove.Remove(obj);
                    continue;
                }

                ObjectsToRemove.Remove(obj);
                InvokeExitEvents(obj);
            }
            ObjectsToRemove.Clear();
        }
        
        private void InvokeEnterEvents(GameObject enteredObject) {
            foreach (var eventAction in OnObjectEnter) {
                eventAction?.Invoke(enteredObject);
            }
        }

        private void InvokeExitEvents(GameObject exitedObject) {
            foreach (var eventAction in OnObjectExit) {
                eventAction?.Invoke(exitedObject);
            }
        }
        
        public void AddObjectEnterEvent(Action<GameObject> eventAction) {
            OnObjectEnter.Add(eventAction);
        }

        public void AddObjectExitEvent(Action<GameObject> eventAction) {
            OnObjectExit.Add(eventAction);
        }

        public List<GameObject> GetObjectsAround() {
            return RegisteredObjects;
        }

    }
}
