using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerState : MonoBehaviour
{

    public PlayerController player;
    public Text state;

  
    void Update()
    {
        SetState();
    }

    void SetState()
    {
        if (player.isRunning)
            state.text = "State : Running";

        if (player.isWalking)
            state.text = "State : Walking";

        if (player.isStanding)
            state.text = "State : Idle";

        if (player.isInAir)
            state.text = "State : In Air / Jumping Up";

        if (player.isFalling && player.isInAir)
            state.text = "State : In Air / Falling Down";

        if (player.isClimbing)
            state.text = "State : Climbing";

        if (player.isSliding)
            state.text = "State : Sliding";

    }
}
