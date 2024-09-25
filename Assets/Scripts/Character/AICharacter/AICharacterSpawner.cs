using UnityEngine;
using Unity.Netcode;

namespace BL
{
    public class AICharacterSpawner : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField] GameObject characterGameObject;
        [SerializeField] GameObject instantiatedGameObject;

        private void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                WorldAIManager.instance.SpawnCharacter(this);
            }
            gameObject.SetActive(false);
        }

        public void AttemptToSpawnCharacter()
        {
            if (characterGameObject != null)
            {
                instantiatedGameObject = Instantiate(characterGameObject, transform.position, transform.rotation);
                var networkObject = instantiatedGameObject.GetComponent<NetworkObject>();
                if (networkObject == null)
                {
                    networkObject = instantiatedGameObject.AddComponent<NetworkObject>();
                }
                networkObject.Spawn();
                Debug.Log("AI Character spawned and networked.");
            }
            else
            {
                Debug.LogError("Character GameObject is null.");
            }
        }
    }
}
