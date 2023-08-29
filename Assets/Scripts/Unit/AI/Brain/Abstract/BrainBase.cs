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

            if (brainComponents.pathfinder != null) 
                brainComponents.pathfinder.Brain = this;
        }

        private void InitializeComponents() {
            Transform componentsParent = transform.Find("BrainComponents");
            
            // Adding Pathfinder component to an Empty GameObject and setting its parent
            GameObject emptyPathfinderObject = new GameObject("Pathfinder");
            emptyPathfinderObject.transform.SetParent(componentsParent);
            brainComponents.pathfinder = emptyPathfinderObject.GetComponent<Pathfinder>();
            // brainComponents.pathfinder.unitRangeDistance = thisUnit.stats.attackRange;
            
            GameObject emptyControllerObject = new GameObject("Controller");
            emptyControllerObject.transform.SetParent(componentsParent);
            brainComponents.controller = emptyControllerObject.GetComponent<ControllerBase>();
            // Pass movement stats
            brainComponents.controller.unitMoveStats = thisUnit.stats.UnitMoveStats;
            brainComponents.controller.rb = GetComponent<Rigidbody2D>();
            brainComponents.controller.col = GetComponent<BoxCollider2D>();
            
            // ... More component to add here
        }

        private void Update() {
            ReadSensorSignals();
        }

        private void ReadSensorSignals() {
            BrainSignal readSignal;
            
            try { readSignal = signalBuffer.GetSignal(); } 
            catch (Exception e) { return; }

            switch (readSignal.type) {
                case BrainSignalType.Vision:
                    objectsAround.Add(readSignal.objects[0]);
                    break;
                case BrainSignalType.Navigation:
                    brainComponents.controller.ReceiveActionRequest(readSignal.action);
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
    }
    
}
