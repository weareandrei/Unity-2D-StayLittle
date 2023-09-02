using System;
using System.Collections.Generic;
using Unit.Util;
using UnityEngine;

namespace Unit.AI.Sensor {
    
    public class VisionSensor : SensorBase {

        // public SensorType Type { get; } = SensorType.Vision;
        // [SerializeField] private Collider2D visionCollider;
        
        // Surrounding states
        private List<GameObject> UnregisteredObjects = new List<GameObject>();
        private List<GameObject> RegisteredObjects = new List<GameObject>();
        private List<GameObject> ObjectsToRemove = new List<GameObject>();
        
        // Events to handle when object enters and exits
        private List<Action<GameObject>> OnObjectEnter = new List<Action<GameObject>>();
        private List<Action<GameObject>> OnObjectExit = new List<Action<GameObject>>();
        
        public override void ScanSurrounding() {
            for (int i = 0; i < UnregisteredObjects.Count; i++) {
                GameObject obj = UnregisteredObjects[i];
                
                if (RegisteredObjects.Contains(obj)) {
                    UnregisteredObjects.Remove(obj);
                    continue;
                }

                RegisteredObjects.Add(obj);
                SendSignalToBrain();
                InvokeEnterEvents(obj);
            }
            UnregisteredObjects.Clear();

            for (int i = 0; i < UnregisteredObjects.Count; i++) {
                GameObject obj = UnregisteredObjects[i];
                if (!RegisteredObjects.Contains(obj)) {
                    ObjectsToRemove.Remove(obj);
                    continue;
                }

                ObjectsToRemove.Remove(obj);
                SendSignalToBrain();
                InvokeExitEvents(obj);
            }
            ObjectsToRemove.Clear();
        }
        
        public override void SendSignalToBrain(string param = null) {
            Brain.ReceiveSignal(
                new BrainSignal (
                    BrainSignalType.Vision,
                    new List<GameObject>(RegisteredObjects)
                )
            );
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!RegisteredObjects.Contains(other.gameObject)) {
                UnregisteredObjects.Add(other.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            if (RegisteredObjects.Contains(other.gameObject)) {
                ObjectsToRemove.Add(other.gameObject);
            }
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

    }
    
}