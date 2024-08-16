using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{

    public class PlayerManager : CharacterManager
    {
        PlayerLocomotionManager playerLocomotionManager;
        // Start is called before the first frame update
        protected override void Awake()
        {
            base.Awake();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base .Update();
            if(!IsOwner)
                return;
            playerLocomotionManager.HandleAllMovement();
        }
    }
}