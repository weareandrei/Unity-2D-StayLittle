using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    public class PlayerBrain : BrainBase {
        protected void FixedUpdate() {
            base.FixedUpdate();
            RecordPlayerInputs();
        }

        private void RecordPlayerInputs() {
            float moveDirection = Input.GetAxisRaw("Horizontal");
            
            if (moveDirection != 0) {
                
                switch (moveDirection) {
                    case > 0:
                        ReceiveSignal(
                            new BrainSignal(
                                BrainSignalType.Navigation,
                                UnitAction.MoveRight
                            )   
                        );
                        break;
                    case < 0:
                        ReceiveSignal(
                            new BrainSignal(
                                BrainSignalType.Navigation,
                                UnitAction.MoveLeft
                            )   
                        );
                        break;
                }
                
                if (Input.GetKeyDown(KeyCode.Space)) {
                    ReceiveSignal(
                        new BrainSignal(
                            BrainSignalType.Navigation,
                            UnitAction.Jump
                        )   
                    );
                };
            }
        }
    }
}