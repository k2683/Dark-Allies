using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Unity.Netcode;

namespace BL
{
    public class CharacterManager : NetworkBehaviour
    {
        [Header("status")]
        public NetworkVariable<bool> isDead = new NetworkVariable<bool>(false,NetworkVariableReadPermission.Everyone,NetworkVariableWritePermission.Owner);
        [HideInInspector] public CharacterController charactercontroller;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterNetworkManager characterNetworkmanager;
        [HideInInspector] public CharacterEffectsManager characterEffectsmanager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorsmanager;
        [Header("Flags")]
        public bool isPerformingActions = false;
        public bool isJumping = false;
        public bool isGrounded = true;
        public bool canRotate = true;
        public bool canMove = true;
        public bool applyRootMotion = false;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            charactercontroller = GetComponent<CharacterController>();
            characterNetworkmanager= GetComponent<CharacterNetworkManager>();
            animator = GetComponent<Animator>();
            characterEffectsmanager = GetComponent<CharacterEffectsManager>();
            characterAnimatorsmanager = GetComponent<CharacterAnimatorManager>();
        }

        protected virtual void Start()
        {
            IgnoreMyOwnColliders();
        }

        protected virtual void Update()
        {
            animator.SetBool("isGrounded", isGrounded);
            if(IsOwner)
            {
                characterNetworkmanager.networkPosition.Value=transform.position;
                characterNetworkmanager.networkRotation.Value=transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp
                    (transform.position, 
                    characterNetworkmanager.networkPosition.Value, 
                    ref characterNetworkmanager.networkPositionVelocity, 
                    characterNetworkmanager.networkPositionSmoothTime);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characterNetworkmanager.networkRotation.Value,
                    characterNetworkmanager.networkRotationSmoothTime);
            }
        }
        protected virtual void LateUpdate()
        {

        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation =false)
        {
            if(IsOwner)
            {
                characterNetworkmanager.currentHealth.Value = 0;
                isDead.Value = true;
            }
            if(!manuallySelectDeathAnimation)
            {
                characterAnimatorsmanager.PlayerTargetActionAnimation("Dead_01", true);
            }
            yield return new WaitForSeconds(5);
        }
    
        public virtual void ReviveCharacter()
        {

        }
        protected virtual void IgnoreMyOwnColliders()
        {
            Collider characterControllerCollider =GetComponent<Collider>();
            Collider[] damagebleCharacterColliders = GetComponentsInChildren<Collider>();
            List<Collider> ignoreColliders = new List<Collider>();
            foreach(var collider in damagebleCharacterColliders)
            {
                ignoreColliders.Add(collider);
            }
            ignoreColliders.Add(characterControllerCollider);
            foreach(var collider in ignoreColliders)
            {
                foreach(var otherCollider in ignoreColliders)
                {
                    Physics.IgnoreCollision(collider, otherCollider,true);
                }
            }
        }
    }
}