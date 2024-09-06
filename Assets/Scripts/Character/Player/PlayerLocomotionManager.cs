using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        [Header("Jump")]
        [SerializeField] float jumpStaminaCost = 25f;
        [SerializeField] float jumpHeight = 1;
        [SerializeField] float jumpForwardSpeed = 5f;
        [SerializeField] float freeFallSpeed = 2f;
        private Vector3 jumpDirection ;
        [Header("Dodge")]
        private Vector3 rollDirection;
        [SerializeField] float dodgeStaminaCost = 25f;
        public void HandleAllMovement()
        {
            HandleAllGroundMovement();
            HandleRotation();
            HandleJumpingMovement();
            HandleFreeFallMovement();
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
                player.characterNetworkmanager.verticalMovement.Value = verticalMovement;
                player.characterNetworkmanager.horizontalMovement.Value = horizontalMovement;
                player.characterNetworkmanager.moveAmount.Value = moveAmount;
            }
            else
            {
                moveAmount = player.characterNetworkmanager.moveAmount.Value;
                verticalMovement =player.characterNetworkmanager.verticalMovement.Value;
                horizontalMovement = player.characterNetworkmanager.horizontalMovement.Value;

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
            if (!player.isGrounded)
                return;
            player.playerAnimatorManager.PlayerTargetActionAnimation("Main_Jump_01", false);

            player.isJumping = true;
            player.playerNetworkManager.currentStamina.Value -= jumpStaminaCost;
            jumpDirection = PlayerCamera.instance.cameraObject.transform.forward * PlayerInputManager.instance.verticalInput;
            jumpDirection += PlayerCamera.instance.cameraObject.transform.right * PlayerInputManager.instance.horizontalInput;
            jumpDirection.y = 0;
            if (jumpDirection != Vector3.zero)
            {
                if (player.playerNetworkManager.isSprinting.Value)
                {
                    jumpDirection *= 1.5f;
                }
                else if (PlayerInputManager.instance.moveAmount > 0.5)
                {
                    jumpDirection *= 0.75f;
                }
                else if (PlayerInputManager.instance.moveAmount <= 0.5)
                {
                    jumpDirection *= 0.5f;
                }
            }
        }
        public void ApplyJumpingVelocity()
        {
            yVelocity.y = Mathf.Sqrt(jumpHeight*-2 *gravityForce);
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
        public void HandleJumpingMovement()
        {
            if(player.isJumping)
            {
                player.charactercontroller.Move(jumpDirection * jumpForwardSpeed * Time.deltaTime);
            }
        }
        public void HandleFreeFallMovement()
        {
            if(!player.isGrounded)
            {
                Vector3 freeFallDirection;
                freeFallDirection = PlayerCamera.instance.transform.forward * PlayerInputManager.instance.verticalInput;
                freeFallDirection += PlayerCamera.instance.transform.right * PlayerInputManager.instance.horizontalInput;
                freeFallDirection.y = 0;
                player.charactercontroller.Move(freeFallDirection * freeFallSpeed * Time.deltaTime);
            }
        }
    }
}