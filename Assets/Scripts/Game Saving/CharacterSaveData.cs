using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex;
        [Header("Character Name")]
        public string characterName = "Character";
        [Header("Time Plaeyer")]
        public float secondsPlayed;
        [Header("World Coordinates")]
        public float xposition;
        public float yposition;
        public float zposition;
        [Header("Resources")]
        public int currentHealth;
        public float currentStamina;
        [Header("Stats")]
        public int vitality;
        public int endurance;

        public void CreateNewBasicPlayerData()
        {
            secondsPlayed = 0f;
            xposition = 0f;
            yposition = 0f;
            zposition = 0f;
            currentHealth = 200;
            currentStamina = 100;
            vitality = 10;
            endurance = 10;
        }
    }
}