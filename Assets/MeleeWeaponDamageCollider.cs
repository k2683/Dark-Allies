using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager CharacterCausingDamage;
    }
}