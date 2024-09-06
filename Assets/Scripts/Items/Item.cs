using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BL
{
    public class Item : ScriptableObject
    {
        [Header("Item Information")]
        public string itemName;
        public string itemIcon;
        [TextArea] public string itemDescription;
        public int itemID;

    }
}