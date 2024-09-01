using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace BL
{ 
    public class CharacterAnimatorManager : MonoBehaviour
    {
        CharacterManager character;
        int vertical;
        int horizontal;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();

            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");

        }
        public void updateAnimatorMovementParameters(float horizontalMovement,float verticleMovement,bool isSprinting)
        {
            float horizontalAmount = horizontalMovement;
            float verticleAmount = verticleMovement; 
            if (isSprinting)
            {
                verticleAmount = 2;
            }
            character.animator.SetFloat(horizontal, horizontalAmount, 0.1f, Time.deltaTime);
            character.animator.SetFloat(vertical, verticleAmount, 0.1f, Time.deltaTime);

        }
        public virtual void PlayerTargetActionAnimation(string targetAnimation, 
            bool isPerformingAction, 
            bool applyRootMotion = true, 
            bool canRotate = false,
            bool canMove = false)
        {
            character.applyRootMotion = applyRootMotion;
            character.animator.CrossFade(targetAnimation, 0.2f);
            character.isPerformingActions = isPerformingAction;
            character.canRotate = canRotate;
            character.canMove = canMove;

            character.characternetworkmanager.NotifyTheServerOfActionAnimationServerRpc(NetworkManager.Singleton.LocalClientId, targetAnimation, applyRootMotion);
        }
    }
}