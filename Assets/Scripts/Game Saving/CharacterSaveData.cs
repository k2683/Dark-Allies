using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Character Name")]
        public string characterName = "Character";
        [Header("Time Plaeyer")]
        public float secondsPlayed;
        [Header("World Coordinates")]
        public float xposition;
        public float yposition;
        public float zposition;

    }
}