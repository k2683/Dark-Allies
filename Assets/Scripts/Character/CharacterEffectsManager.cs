using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        CharacterManager character;
        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void ProcessInstantEffect(InstantCharacterEffects effect)
        {
            effect.ProcessEffect(character);
        }
    }
}