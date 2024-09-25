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
                // �����ͻ��������¼�
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnectedCallback;
            }
        }

        private void OnDestroy()
        {
            if (IsServer && NetworkManager.Singleton != null)
            {
                // �Ƴ���������ֹ�ڴ�й©
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnectedCallback;
            }
        }

        private void OnClientConnectedCallback(ulong clientId)
        {
            // ���¿ͻ�������ʱ�����Ѿ����ڵ� AI ��ɫͬ�����¿ͻ���
            SyncExistingAICharactersToClient(clientId);
        }

        private void SyncExistingAICharactersToClient(ulong clientId)
        {
            // ��ȡ�����Ѿ����ڵ� AI ��ɫ
            foreach (var aiCharacter in FindObjectsOfType<AICharacterManager>())
            {
                var networkObject = aiCharacter.GetComponent<NetworkObject>();

                if (networkObject != null)
                {
                    // ������������Ƿ��Ѿ�������������
                    if (!networkObject.IsSpawned)
                    {
                        // ���δ���ɣ����ڷ����������ɲ�ͬ�������пͻ���
                        networkObject.Spawn();
                    }

                    // ���¿ͻ������Ϊ�۲���
                    networkObject.NetworkShow(clientId);
                }
            }
        }
    }
}
