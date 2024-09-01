using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BL
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager instance;
        [Header("NETWORK JOIN")]
        [SerializeField] bool startGameAsClient;
        [HideInInspector] public PlayerUIHudManager playerUIHudManager;
        public void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
            playerUIHudManager = GetComponentInChildren<PlayerUIHudManager>();
        }
        private void Start()
        {
            DontDestroyOnLoad (gameObject);
        }
        public void Update()
        {
            if(startGameAsClient)
            {
                startGameAsClient = false;
                NetworkManager.Singleton.Shutdown();
                NetworkManager.Singleton.StartClient();
            }
        }

    }
}