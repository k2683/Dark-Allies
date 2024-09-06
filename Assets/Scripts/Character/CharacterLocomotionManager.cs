using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        CharacterManager character;
        [Header("Ground Check & Jumping")]
        [SerializeField] protected float gravityForce = -95.5f;
        [SerializeField] LayerMask groundLayer;
        [SerializeField] float groundCheckSphereRadius = 0.3f;
        [SerializeField] protected Vector3 yVelocity;
        [SerializeField] protected float groundedYVelocity = -20;
        [SerializeField] protected float fallStartYVelocity = -5;
        protected bool fallingVelocityHAsBeenSet = false;
        protected float inAirTimer = 0;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Update()
        {
            HandleGroundCheck();
            if (character.isGrounded)
            {
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHAsBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                if (!character.isJumping && !fallingVelocityHAsBeenSet)
                {
                    fallingVelocityHAsBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }
                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("inAirTime", inAirTimer);
                yVelocity.y += gravityForce * Time.deltaTime;
            }
            //Debug.Log("LOCOyVelocity" + yVelocity);
            character.charactercontroller.Move(yVelocity * Time.deltaTime);


        }
        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

        //Only shows in editor
        
        protected void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(character.transform.position, groundCheckSphereRadius);
        }
    }
}