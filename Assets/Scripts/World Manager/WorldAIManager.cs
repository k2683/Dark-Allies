using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace BL
{
    public class WorldAIManager : MonoBehaviour
    {
        public static WorldAIManager instance;

        [Header("Characters")]
        [SerializeField] List<AICharacterSpawner> aiCharacterSpawners;
        [SerializeField] List<GameObject> spawnedInCharacters;

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

        public void SpawnCharacter(AICharacterSpawner aiCharacterSpawner)
        {
            if (NetworkManager.Singleton.IsServer)
            {
                aiCharacterSpawners.Add(aiCharacterSpawner);
                aiCharacterSpawner.AttemptToSpawnCharacter();
            }
        }

        private void DespawnAllCharacters()
        {
            foreach (var character in spawnedInCharacters)
            {
                character.GetComponent<NetworkObject>().Despawn();
            }
        }

        private void DisableAllCharacters()
        {
            // TO DO DISABLE CHARACTER GAMEOBJECTS, SYNC DISABLED STATUS ON NETWORK
            // DISABLE GAMEOBJECTS FOR CLIENTS UPON CONNECTING, IF DISABLED STATUS IS TRUE
            // CAN BE USED TO DISABLE CHARACTERS THAT ARE FAR FROM PLAYERS TO SAVE MEMORY
            // CHARACTERS CAN BE SPLIT INTO AREAS (AREA_00_, AREA_01, AREA_02) ECT
        }
    }
}
