using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BL
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager instance;
        PlayerControls playerControls;
        public PlayerManager player;

        [Header("Player Movement input")]
        [SerializeField] Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;


        [Header("Camera Movement input")]
        [SerializeField] Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        [Header("Player Action Input")]
        [SerializeField] bool dodgeInput = false;
        [SerializeField] bool sprintInput = false;
        [SerializeField] bool jumpInput = false;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        private void OnSceneChange(Scene oldscene,Scene newScene)
        {
            if(newScene.buildIndex==WorldSaveGameManager.Instance.GetWorldSceneIndex())
            {
                instance.enabled = true;    
            }
            else
            {
                instance.enabled = false;   
            }
        }
        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            instance.enabled = false;
        }
        private void Update()
        {
            HandleAllInputs();
        }
        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls=new PlayerControls();

                playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;


                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

            }
            playerControls.Enable();
        }
        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprinting();
            HandleJumpInput();
        }
        private void HandlePlayerMovementInput()
        {
            horizontalInput=movementInput.x;
            verticalInput=movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
            if(moveAmount<0.5&&moveAmount>0)
            {
                moveAmount = 0.5f;
            }
            else if(moveAmount<1&&moveAmount>0.5)
            {
                moveAmount = 1.0f;
            }
            if (player == null)
                return;
            //If not locked on. Only use moveAmount
            player.playerAnimatorManager.updateAnimatorMovementParameters(0, moveAmount,player.playerNetworkManager.isSprinting.Value);

            //If locked on
        }
        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                if(focus)
                {
                    playerControls.Enable();
                }
                else
                {
                    playerControls.Disable();
                }
            }
        }
        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }

        private void HandleDodgeInput()
        {
            if(dodgeInput)
            {

                dodgeInput = false;
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }
        private void HandleSprinting()
        {
            if(sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.playerNetworkManager.isSprinting.Value = false;
            }
        }
        private void HandleSprintInput()
        {
            if(jumpInput)
            {
                jumpInput = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
        private void HandleJumpInput()
        {
            if(jumpInput)
            {
                jumpInput = false;
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }
    }
}
