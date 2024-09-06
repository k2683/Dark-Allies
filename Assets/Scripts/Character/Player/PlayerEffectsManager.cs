using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("Debug")]
        [SerializeField] InstantCharacterEffects effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                InstantCharacterEffects effect = Instantiate(effectToTest) ;
                ProcessInstantEffect(effect);
            }
        }
    }
}