using Unit.Util;
using UnityEngine;

namespace Unit.AI {
    public class PlayerBrain : BrainBase {
        protected void Update() {
            RecordPlayerInputs();
        }

        private void RecordPlayerInputs() {
            float moveDirection = Input.GetAxisRaw("Horizontal");
            
            switch (moveDirection) {
                case 0:
                    ReceiveSignal(
                        new BrainSignal(
                            BrainSignalType.Navigation,
                            UnitAction.StopMovement
                        )
                    );
                    break;
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
            }
            
            if (Input.GetKeyDown(KeyCode.E)) {
                ReceiveSignal(
                    new BrainSignal(
                        BrainSignalType.Combat,
                        UnitAction.Attack,
                        "LightAttack"
                    )   
                );
            }
        }
    }
}