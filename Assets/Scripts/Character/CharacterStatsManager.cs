using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace BL
{
    public class CharacterStatsManager : MonoBehaviour
    {
        CharacterManager character;
        [Header("Stamina Regeneration")]
        [SerializeField] float staminaRegenarationAmount = 2.0f;
        private float staminaRegenarationTimer = 0f;
        private float staminaTickTime = 0f;
        [SerializeField] float regenarationDelay = 2.0f;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }
        protected virtual void Start()
        {

        }
        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            int stamina = 0;
            stamina = endurance * 10;
            return stamina;
        }
        public int CalculateHealthBasedOnVitalityLevel(int vitality)
        {
            int health = 0;
            health = vitality * 20;
            return health;
        }


        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;

            if (character.characterNetworkmanager.isSprinting.Value)
                return;

            if (character.isPerformingActions)
                return;
            staminaRegenarationTimer += Time.deltaTime;
            if (staminaRegenarationTimer >= regenarationDelay)
            {
                if (character.characterNetworkmanager.currentStamina.Value < character.characterNetworkmanager.maxStamina.Value)
                {
                    staminaTickTime = staminaTickTime + Time.deltaTime;
                    if (staminaTickTime >= 0.1)
                    {
                        staminaTickTime = 0;
                        character.characterNetworkmanager.currentStamina.Value += staminaRegenarationAmount;
                    }
                }
            }
        }

        public virtual void ResetStaminaRegenTimer(float previousStaminaAmount,float currentStaminaAmount)
        {
            if(currentStaminaAmount < previousStaminaAmount)
            {
                staminaRegenarationTimer = 0;
            }
            //staminaRegenarationTimer = regenarationDelay;
        }
    }
}