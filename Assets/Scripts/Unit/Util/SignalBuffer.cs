using System;
using System.Collections.Generic;
using Unit.AI;

namespace Unit.Util {
    public class SignalBuffer {
        private Queue<BrainSignal> buffer;

        public SignalBuffer() {
            buffer = new Queue<BrainSignal>();
        }

        public void AddSignal(BrainSignal signal) {
            buffer.Enqueue(signal);
        }

        public BrainSignal GetSignal() {
            if (buffer.Count > 0) {
                return buffer.Dequeue();
            }
            
            throw new InvalidOperationException("No signals in the buffer");
        }
    }
}