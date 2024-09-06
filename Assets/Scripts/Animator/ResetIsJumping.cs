using BL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class ResetIsJumping : StateMachineBehaviour
    {
        CharacterManager character;

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (character == null)
            {
                character = animator.GetComponent<CharacterManager>();
            }
            character.isJumping = false;

        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

        }

    }
}