using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float moveAmount;
        [Header("Movement Settings")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runnningSpeed = 5;
        [SerializeField] float sprintingSpeed = 6.5f;
        [SerializeField] float rotationSpeed = 15;
        [SerializeField] float sprintingStaminaCost = 2.0f;
        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25f;
        [SerializeField] float jumpStaminaCost = 25f;
        public void HandleAllMovement()
        {
            //if(player.isPerformingActions) 
            //    return;
            HandleAllGroundMovement();
            HandleRotation();
        }
        public void HandleAllGroundMovement()
        {
            if(!player.canMove)
                return;

            GetMovementValues();
            moveDirection = PlayerCamera.instance.transform.forward * verticalMovement;
            moveDirection = moveDirection+PlayerCamera.instance.transform.right*horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.charactercontroller.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                //running
                if (PlayerInputManager.instance.moveAmount > 0.5f)
                {

                    player.charactercontroller.Move(moveDirection * runnningSpeed * Time.deltaTime);
                }
                //walking
                else if (PlayerInputManager.instance.moveAmount < 0.5f && PlayerInputManager.instance.moveAmount > 0)
                {
                    player.charactercontroller.Move(moveDirection * walkingSpeed * Time.deltaTime);

                }

            }
        }
        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
            moveAmount = PlayerInputManager.instance.moveAmount;
        }
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

        }
        protected override void Update()
        {
            base.Update();
            if(player.IsOwner)
            {
                player.characternetworkmanager.verticalMovement.Value = verticalMovement;
                player.characternetworkmanager.horizontalMovement.Value = horizontalMovement;
                player.characternetworkmanager.moveAmount.Value = moveAmount;
            }
            else
            {
                moveAmount = player.characternetworkmanager.moveAmount.Value;
                verticalMovement =player.characternetworkmanager.verticalMovement.Value;
                horizontalMovement = player.characternetworkmanager.horizontalMovement.Value;

                player.playerAnimatorManager.updateAnimatorMovementParameters(0, moveAmount, player.playerNetworkManager.isSprinting.Value);
            }
        }
        private void HandleRotation()
        {
            if(!player.canRotate)
                return;
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward*verticalMovement;
            targetRotationDirection = targetRotationDirection +PlayerCamera.instance.transform.right*horizontalMovement;
            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;
            if(targetRotationDirection == Vector3.zero)
            {
                targetRotationDirection = transform.forward;
            }
            Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = targetRotation;
        }
        public void AttemptToPerformDodge()
        {
            if(player.isPerformingActions)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if(PlayerInputManager.instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
                rollDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();
                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;
                player.playerAnimatorManager.PlayerTargetActionAnimation("Roll_Forward_01", true,true);
            }
            else
            {
                player.playerAnimatorManager.PlayerTargetActionAnimation("Back_Step_01",true,true);
            }
            player.playerNetworkManager.currentStamina.Value -=dodgeStaminaCost;
        }
        public void AttemptToPerformJump()
        {
            if (player.isPerformingActions)
                return;
            if (player.playerNetworkManager.currentStamina.Value <= 0)
                return;
            if (player.isJumping)
                return;
            if(player.isGrounded)
                return;
            player.playerAnimatorManager.PlayerTargetActionAnimation("Main_Jump_01",false);

            player.isJumping = true;
            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;

        }
        public void ApplyJumpingVelocity()
        {

        }
        public void HandleSprinting()
        {
            if(player.isPerformingActions)
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
            if(player.playerNetworkManager.currentStamina.Value<=0)
            {   
                player.playerNetworkManager.isSprinting.Value = false;
                return;
            }
            if (moveAmount >= 0.5)
            {
                player.playerNetworkManager.isSprinting.Value = true;

            }
            else
                player.playerNetworkManager.isSprinting.Value = false;
            if(player.playerNetworkManager.isSprinting.Value)
            {
                player.playerNetworkManager.currentStamina.Value -= sprintingStaminaCost * Time.deltaTime;
            }
        }
    }
}