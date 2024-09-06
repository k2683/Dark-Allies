using BL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BL
{
    public class InstantCharacterEffects : ScriptableObject
    {
        [Header("Effect ID")]
        public int instantEffectID;
        //public CharacterManager character;
        public virtual void ProcessEffect(CharacterManager character)
        {

        }
    }
}