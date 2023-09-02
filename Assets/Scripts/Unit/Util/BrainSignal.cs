using System;
using System.Collections.Generic;
using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    
    [Serializable]
    public struct BrainSignal {

        [SerializeField]
        public BrainSignalType type;

        // Possible data types that Signal could hold
        [SerializeField]
        public List<GameObject> objects;
        public Vector2 direction;
        [SerializeField]
        public UnitAction action;

        public BrainSignal(BrainSignalType type, List<GameObject> objects) {
            this.type = type;
            this.objects = objects;
            this.direction = new Vector2(0,0);
            this.action = UnitAction.None;
        }

        public BrainSignal(BrainSignalType type, Vector2 direction) {
            this.type = type;
            this.objects = null;
            this.direction = direction;
            this.action = UnitAction.None;
        }

        public BrainSignal(BrainSignalType type, UnitAction action) {
            this.type = type;
            this.objects = null;
            this.direction = new Vector2(0,0);
            this.action = action;
        }
        
        public bool Equals(BrainSignal other) {
            if (type != other.type) {
                return false;
            }

            return type switch {
                BrainSignalType.Vision => GameObjectListsEqual(objects, other.objects),
                BrainSignalType.Navigation => direction == other.direction && action == other.action,
            };
        }

        private bool GameObjectListsEqual(List<GameObject> list1, List<GameObject> list2) {
            if (list1.Count != list2.Count) return false;
            
            for (int i = 0; i < list1.Count; i++) {
                if (list1[i] != list2[i]) {
                    return false;
                }
            }

            return true;
        }

    }

}