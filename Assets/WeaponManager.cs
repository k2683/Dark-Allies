using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] MeleeWeaponDamageCollider meleeDamageCollider;
        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }
        public void SetWeaponDamage(CharacterManager characterWieldingWeapon,WeaponItem weapon)
        {
            meleeDamageCollider.CharacterCausingDamage = characterWieldingWeapon;
            meleeDamageCollider.physicalDamage = weapon.physicalDamage;
            meleeDamageCollider.magicDamage = weapon.magicDamage;
            meleeDamageCollider.fireDamage = weapon.fireDamage;
            meleeDamageCollider.lightningDamage = weapon.lightningDamage;
            meleeDamageCollider.holyDamage = weapon.holyDamage;
        }
    }
}