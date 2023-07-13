using System;
using UnityEngine;

namespace Interaction {
    public class SimpleMovementController : MonoBehaviour {
        public event Action moveActions;

        private bool actionInvoked = false;

        public bool ActionInvoked {
            get { return actionInvoked; }
            set {
                actionInvoked = value;
                if (actionInvoked) {
                    // Trigger the event when movingDown is set to true
                    moveActions?.Invoke();
                }
            }
        }

    }
}