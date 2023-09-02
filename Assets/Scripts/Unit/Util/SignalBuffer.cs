using System;
using System.Collections.Generic;
using Codice.CM.Common.Selectors;
using Unit.AI;
using UnityEngine;

namespace Unit.Util {
    
    [Serializable]
    public class SignalBuffer {
        private Queue<BrainSignal> buffer;

        [SerializeField] private List<BrainSignal> bufferCopy;

        public SignalBuffer() {
            buffer = new Queue<BrainSignal>();
        }

        public void AddSignal(BrainSignal signal) {
            buffer.Enqueue(signal);
            bufferCopy = new List<BrainSignal>(buffer);
        }

        public BrainSignal GetSignal() {
            if (buffer.Count > 0) {
                return buffer.Dequeue();
            }
            
            throw new InvalidOperationException("No signals in the buffer");
        }

        public bool IsSignalInQueue(BrainSignal incomingSignal) {
            foreach (BrainSignal signal in buffer) {
                if (signal.Equals(incomingSignal)) {
                    return true;
                }
            }
    
            return false;
        }

    }
}