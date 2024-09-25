using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BL
{
    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager instance;

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip rollSFX;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            Debug.Log("physicalDamageSFX" + physicalDamageSFX.Length);
            Debug.Log("array.length="+array.Length);
            int index = Random.Range(0, array.Length);

            return array[index];
        }
    }
}
