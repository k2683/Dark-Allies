using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace BL
{
    public class PlayerStatsManager : CharacterStatsManager
    {
        PlayerManager player;
        protected override void Awake()
        {
            base.Awake();
            player = GetComponent<PlayerManager>();
        }
        protected override void Start()
        {
            base.Start();
            //创建character creation menu的时候需要计算一次，load存档的话就会覆盖掉
            //CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value); 
            //CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            
            player.playerNetworkManager.maxHealth.Value = CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            player.playerNetworkManager.maxStamina.Value = CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
            player.playerNetworkManager.currentHealth.Value = CalculateHealthBasedOnVitalityLevel(player.playerNetworkManager.vitality.Value);
            player.playerNetworkManager.currentStamina.Value = CalculateStaminaBasedOnEnduranceLevel(player.playerNetworkManager.endurance.Value);
        
        }
    }
}