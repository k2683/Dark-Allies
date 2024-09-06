using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    [CreateAssetMenu(menuName ="Character Effects/Instant Effects/Tke Stamina Damage")]
    public class TakeStaminaDamageEffect : InstantCharacterEffects
    {
        public float staminaDamage;
        public override void ProcessEffect(CharacterManager character)
        {
            CalculateStaminaDamage(character);
        }
        private void CalculateStaminaDamage(CharacterManager character)
        {
            if(character.IsOwner)
            {
                
                character.characterNetworkmanager.currentStamina.Value -= staminaDamage;
            }
        }
    }
}
