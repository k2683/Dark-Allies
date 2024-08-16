using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        PlayerManager player;
        public float horizontalMovement;
        public float verticallMovement;
        public float moveAmount;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] float walkingSpeed = 2;
        [SerializeField] float runnningSpeed = 5;
        [SerializeField] float rotationSpeed = 15;
        public void HandleAllMovement()
        {
            HandleAllGroundMovement();
            HandleRotation();
        }
        public void HandleAllGroundMovement()
        {
            GetVerticleAndHorizontalInputs();
            moveDirection = PlayerCamera.instance.transform.forward * verticallMovement;
            moveDirection = moveDirection+PlayerCamera.instance.transform.right*horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;
            //running
            if(PlayerInputManager.instance.moveAmount>0.5f)
            {

                player.charactercontroller.Move(moveDirection*runnningSpeed*Time.deltaTime);
            }
            //walking
            else if(PlayerInputManager.instance.moveAmount<0.5f&& PlayerInputManager.instance.moveAmount>0)
            {
                player.charactercontroller.Move(moveDirection * walkingSpeed * Time.deltaTime);

            }
        }
        private void GetVerticleAndHorizontalInputs()
        {
            verticallMovement = PlayerInputManager.instance.verticalInput;
            horizontalMovement = PlayerInputManager.instance.horizontalInput;
        }
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();

        }
        private void HandleRotation()
        {
            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.instance.cameraObject.transform.forward*verticallMovement;
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
    }
}