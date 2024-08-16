using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Unity.Netcode;

namespace BL
{
    public class CharacterManager : NetworkBehaviour
    {
        public CharacterController charactercontroller;
        CharacterNetworkManager characternetworkmanager;
        protected virtual void Awake()
        {
            DontDestroyOnLoad(this);
            charactercontroller = GetComponent<CharacterController>();
            characternetworkmanager= GetComponent<CharacterNetworkManager>();
        }
        protected virtual void Update()
        {
            if(IsOwner)
            {
                characternetworkmanager.networkPosition.Value=transform.position;
                characternetworkmanager.networkRotation.Value=transform.rotation;
            }
            else
            {
                transform.position = Vector3.SmoothDamp
                    (transform.position, 
                    characternetworkmanager.networkPosition.Value, 
                    ref characternetworkmanager.networkPositionVelocity, 
                    characternetworkmanager.networkPositionSmoothTime);
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    characternetworkmanager.networkRotation.Value,
                    characternetworkmanager.networkRotationSmoothTime);
            }
        }
    }
}