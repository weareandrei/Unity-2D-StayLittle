using System.Collections.Generic;
using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    
    public struct BrainSignal {

        public BrainSignalType type;

        // Possible data types that Signal could hold
        public List<GameObject> objects;
        public Vector2 direction;
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
    }



}