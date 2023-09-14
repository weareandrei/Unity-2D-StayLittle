using System;
using System.Collections.Generic;
using Unit.AI.Navigation;
using Unit.Base;
using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    
    public abstract class BrainBase : MonoBehaviour {
        
        protected BaseUnit thisUnit;
        
        private List<ISensor> availableSensors = new (); // List of Sensors in here
        protected BrainComponents brainComponents; // List of available Components
        
        [SerializeField] private SignalBuffer signalBuffer = new ();
        protected List<GameObject> objectsAround = new ();

        protected void Start() {
            thisUnit = GetComponent<BaseUnit>();
            
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

        protected void FixedUpdate() {
            ReadSensorSignals();
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

            try { // Initialize PATHFINDER
                GameObject pathfinderObject = componentsParent.Find("Pathfinder").gameObject;
                brainComponents.pathfinder = pathfinderObject.GetComponent<Pathfinder>();
            } catch (Exception e) {
                Debug.LogError("Pathfinder component not found on " + gameObject.name);
            }

            try { // Initialize CONTROLLER
                GameObject controllerObject = componentsParent.Find("Controller").gameObject;
                brainComponents.controller = controllerObject.GetComponent<ControllerBase>();
                
                brainComponents.controller.unitMoveStats = thisUnit.stats.UnitMoveStats;
                brainComponents.controller.rb = GetComponent<Rigidbody2D>();
                brainComponents.controller.col = GetComponent<BoxCollider2D>();
                
                if (brainComponents.controller.rb == null || brainComponents.controller.col == null) {
                    Debug.LogError("Rigidbody2D or BoxCollider2D not found on " + gameObject.name);
                }
            } catch (Exception e) {
                Debug.LogError("Controller component not found on " + gameObject.name);
            }
            
            try { // Initialize COMBAT COMPONENT
                GameObject combatObject = componentsParent.Find("Combat").gameObject;
                brainComponents.combatComponent = combatObject.GetComponent<CombatComponent>();
            } catch (Exception e) {
                Debug.LogError("CombatComponent component not found on " + gameObject.name);
            }

            // ... More component checks to add here
        }
        
        private void ReadSensorSignals() {
            BrainSignal receivedSignal;
            
            try { receivedSignal = signalBuffer.GetSignal(); }
            catch (Exception e) { return; }

            switch (receivedSignal.type) {
                case BrainSignalType.Vision:
                    objectsAround = new List<GameObject>(receivedSignal.objects);
                    break;
                case BrainSignalType.Navigation:
                    if (brainComponents.controller) {
                        brainComponents.controller.ReceiveActionRequest(receivedSignal.action);
                    }
                    break;
                case BrainSignalType.Combat:
                    if (brainComponents.combatComponent) {
                        brainComponents.combatComponent.PerformAttack(receivedSignal.param);    
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void ReceiveSignal(BrainSignal incomingSignal) {
            if (!signalBuffer.IsSignalInQueue(incomingSignal)) {
                signalBuffer.AddSignal(incomingSignal);
            }
        }
        
    }

    public struct BrainComponents {
        public Pathfinder pathfinder;
        public ControllerBase controller;
        public CombatComponent combatComponent;
    }
    
}
