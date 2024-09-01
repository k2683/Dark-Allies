using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class PlayerAnimatorManager : CharacterAnimatorManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>(); 
        }
        private void OnAnimatorMove()
        {
            if(player.applyRootMotion)
            {
                Vector3 velocity = player.animator.deltaPosition;
                player.charactercontroller.Move(velocity);
                player.transform.rotation*= player.animator.deltaRotation;
            }
        }
    }

}