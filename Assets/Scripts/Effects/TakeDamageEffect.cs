using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    [CreateAssetMenu(menuName ="Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffects
    {
        [Header("Character Causing Damage")]
        public CharacterManager characterCausingDamage;         
        [Header("Damage")]
        public float physicalDamage = 0;
        public float magicDamage = 0;
        public float fireDamage = 0;
        public float lightningDamage = 0;
        public float holyDamage = 0;
        [Header("Final Damage")]
        private int finalDamageDealt = 0;
        [Header("Poise")]
        public int poiseDamage = 0;
        public bool poiseIsBroken =false;

        [Header("Animation")]
        public bool playDamageAnimation = true;
        public bool manuallySelectDamageAnimation = false;
        public string damageAnimation;

        [Header("Direction Damage Taken From")]
        public float angleHitFrom;
        public Vector3 contactPoint;
        public override void ProcessEffect(CharacterManager character)
        {
            base.ProcessEffect(character);
            if(character.isDead.Value)
            {
                return;
            }
            CalculateDamage(character);
        }
        private void CalculateDamage(CharacterManager character)
        {
            
            if (!character.IsOwner)
                return;
            
            if (characterCausingDamage != null)
            {
            }
            finalDamageDealt = Mathf.RoundToInt(physicalDamage+magicDamage+ fireDamage+ lightningDamage+ holyDamage+poiseDamage);
            if(finalDamageDealt <=0)
            {
                finalDamageDealt=1;
            }
            character.characterNetworkmanager.currentHealth.Value -= finalDamageDealt;
        }
    }
}