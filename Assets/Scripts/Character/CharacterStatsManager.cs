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
        public int CalculateStaminaBasedOnEnduranceLevel(int endurance)
        {
            float stamina = 0;
            stamina = endurance * 10;
            return Mathf.RoundToInt(stamina);
        }
        public virtual void RegenerateStamina()
        {
            if (!character.IsOwner)
                return;

            if (character.characternetworkmanager.isSprinting.Value)
                return;

            if (character.isPerformingActions)
                return;
            staminaRegenarationTimer += Time.deltaTime;
            if (staminaRegenarationTimer >= regenarationDelay)
            {
                if (character.characternetworkmanager.currentStamina.Value < character.characternetworkmanager.maxStamina.Value)
                {
                    staminaTickTime = staminaTickTime + Time.deltaTime;
                    if (staminaTickTime >= 0.1)
                    {
                        staminaTickTime = 0;
                        character.characternetworkmanager.currentStamina.Value += staminaRegenarationAmount;
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