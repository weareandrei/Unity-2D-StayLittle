using System;
using System.Collections.Generic;
using Unit.AI.Navigation;
using Unit.Util;
using UnityEngine;
using UnitBase = Unit.Base;

namespace Unit.AI {
    
    public abstract class BrainBase : MonoBehaviour {
        
        protected UnitBase.Unit thisUnit;
        
        private List<ISensor> availableSensors; // List of Sensors in here
        protected BrainComponents brainComponents; // List of available Components
        
        private SignalBuffer signalBuffer = new ();
        protected List<GameObject> objectsAround = new ();

        protected void Awake() {
            thisUnit = GetComponent<UnitBase.Unit>();
            
            Transform sensorsParent = transform.Find("Sensors");

            if (sensorsParent != null) {
                foreach (Transform sensorTransform in sensorsParent) {
                    ISensor sensor = sensorTransform.GetComponent<ISensor>();
                    if (sensor != null) {
                        availableSensors.Add(sensor);
                    }
                }
            }
            
            ActivateSensorsAndComponents();
        }

        private void ActivateSensorsAndComponents() {
            foreach (ISensor sensor in availableSensors) {
                sensor.Brain = this;
            }

            InitializeComponents();

            // Loop through non-null BrainComponents and assign their Brain property
            if (brainComponents.pathfinder != null) {
                brainComponents.pathfinder.Brain = this;
            }

            // if (brainComponents.controller != null) {
            //     brainComponents.controller.Brain = this;
            // }

            if (brainComponents.combatComponent != null) {
                brainComponents.combatComponent.Brain = this;
            }

            // ... Similar assignments for other components

            // Update code here
        }


        private void InitializeComponents() {
            Transform componentsParent = transform.Find("BrainComponents");
    
            // Adding Pathfinder component to an Empty GameObject and setting its parent
            GameObject emptyPathfinderObject = new GameObject("Pathfinder");
            emptyPathfinderObject.transform.SetParent(componentsParent);
            brainComponents.pathfinder = emptyPathfinderObject.GetComponent<Pathfinder>();
            if (brainComponents.pathfinder == null) {
                Debug.LogError("Pathfinder component not found on emptyPathfinderObject.");
            }
    
            // Adding Controller component to an Empty GameObject and setting its parent
            GameObject emptyControllerObject = new GameObject("Controller");
            emptyControllerObject.transform.SetParent(componentsParent);
            brainComponents.controller = emptyControllerObject.GetComponent<ControllerBase>();
            if (brainComponents.controller == null) {
                Debug.LogError("Controller component not found on emptyControllerObject.");
            } else {
                // Pass movement stats
                brainComponents.controller.unitMoveStats = thisUnit.stats.UnitMoveStats;
                brainComponents.controller.rb = GetComponent<Rigidbody2D>();
                brainComponents.controller.col = GetComponent<BoxCollider2D>();
                if (brainComponents.controller.rb == null || brainComponents.controller.col == null) {
                    Debug.LogError("Rigidbody2D or BoxCollider2D not found on the current GameObject.");
                }
            }
    
            // Adding CombatComponent component to an Empty GameObject and setting its parent
            GameObject emptycombatComponentObject = new GameObject("CombatComponent");
            emptycombatComponentObject.transform.SetParent(componentsParent);
            brainComponents.combatComponent = emptycombatComponentObject.GetComponent<CombatComponent>();
            if (brainComponents.combatComponent == null) {
                Debug.LogError("CombatComponent component not found on emptycombatComponentObject.");
            }
    
            // ... More component checks to add here
        }

        private void Update() {
            ReadSensorSignals();
        }

        private void ReadSensorSignals() {
            BrainSignal receivedSignal;
            
            try { receivedSignal = signalBuffer.GetSignal(); } 
            catch (Exception e) { return; }

            switch (receivedSignal.type) {
                case BrainSignalType.Vision:
                    objectsAround = receivedSignal.objects;
                    break;
                case BrainSignalType.Navigation:
                    brainComponents.controller.ReceiveActionRequest(receivedSignal.action);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ReceiveSignal(BrainSignal incomingSignal) {
            signalBuffer.AddSignal(incomingSignal);
        }
        
    }

    public struct BrainComponents {
        public Pathfinder pathfinder;
        public ControllerBase controller;
        public CombatComponent combatComponent;
    }
    
}
