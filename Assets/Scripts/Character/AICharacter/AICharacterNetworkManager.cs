using UnityEngine;
using Unity.Netcode;

namespace BL
{
    public class AICharacterNetworkManager : CharacterNetworkManager
    {
        private void Start()
        {
            if (IsServer)
            {
                // 监听客户端连接事件
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            }
        }

        private void OnDestroy()
        {
            if (IsServer && NetworkManager.Singleton != null)
            {
                // 移除监听，防止内存泄漏
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            }
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            // 当新客户端连接时，将已经存在的 AI 角色同步给新客户端
            SyncExistingAICharactersToClient(clientId);
        }

        private void SyncExistingAICharactersToClient(ulong clientId)
        {
            // 获取所有已经存在的 AI 角色
            foreach (var aiCharacter in FindObjectsOfType<AICharacterManager>())
            {
                var networkObject = aiCharacter.GetComponent<NetworkObject>();

                if (networkObject != null)
                {
                    // 检查该网络对象是否已经存在于网络中
                    if (!networkObject.IsSpawned)
                    {
                        // 如果未生成，则在服务器上生成并同步给所有客户端
                        networkObject.Spawn();
                    }

                    // 将新客户端添加为观察者
                    networkObject.NetworkShow(clientId);
                }
            }
        }
    }
}
