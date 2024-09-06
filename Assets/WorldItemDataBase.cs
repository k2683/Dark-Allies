using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace BL
{
    public class WorldItemDataBase : MonoBehaviour
    {
        public static WorldItemDataBase Instance;

        public WeaponItem unarmedWeapon;
        [Header("weapons")]
        [SerializeField] List<WeaponItem> weapons = new List<WeaponItem>();
        [Header("Items")]
        private List<Item> items = new List<Item>();
        private void Awake()
        {
            if(Instance == null)
                Instance = this;
            else
                Destroy(Instance);
            foreach (var weapon in weapons)
            {
                items.Add(weapon);
            }
            for(int i=0;i<items.Count; i++)
            {
                items[i].itemID = i;
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(Instance);
        }

        public WeaponItem GetWeaponByID(int ID)
        {
            return weapons.FirstOrDefault(weapons=>weapons.itemID == ID);
        }
    }
}