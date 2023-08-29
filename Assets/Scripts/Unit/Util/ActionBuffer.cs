using System;
using System.Collections.Generic;
using Unit.AI;

namespace Unit.Util {
    public class ActionBuffer {
        
        private Queue<UnitAction> buffer;

        public ActionBuffer() {
            buffer = new Queue<UnitAction>();
        }

        public void AddAction(UnitAction action) {
            buffer.Enqueue(action);
        }

        public UnitAction GetAction() {
            if (buffer.Count > 0) {
                return buffer.Dequeue();
            }
            
            throw new InvalidOperationException("No actions in the buffer");
        }
        
    }

    public enum UnitAction {
        Jump,
        None,
        MoveLeft,
        MoveRight,
        StopMovement
    }
}