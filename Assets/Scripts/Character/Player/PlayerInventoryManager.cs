using BL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;
        [Header("Quick Slots")]
        //初始化的时候记得填满三个，空的填unarmed，不要有null，不然switch的时候会报错
        public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[3];
        public int rightHandWeaponIndex = 0;
        public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[3];
        public int leftHandWeaponIndex = 0;

    }
}