using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    [SerializeField] private Transform m_TestObjectPrefab;

    private NetworkVariable<int> randomNumber = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private NetworkVariable<PlayerData> playerData = new NetworkVariable<PlayerData>(default(PlayerData), NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct PlayerData : INetworkSerializable, System.IEquatable<PlayerData>
    {
        public int Point;
        public int Power;
        public FixedString128Bytes Message;

        public bool Equals(PlayerData other)
        {
            return Point == other.Point && Power == other.Power;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out Point);
                reader.ReadValueSafe(out Power);
                reader.ReadValueSafe(out Message);
            }
            else
            {
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(Point);
                writer.WriteValueSafe(Power);
                writer.WriteValueSafe(Message);
            }
        }


    }





    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += OnValueChanged;
        playerData.OnValueChanged += OnPlayerDataChanged;
        name = "PlayerNetwork_" + OwnerClientId.ToString();
    }
    public override void OnNetworkDespawn()
    {
        randomNumber.OnValueChanged -= OnValueChanged;
        playerData.OnValueChanged += OnPlayerDataChanged;

    }

    void OnValueChanged(int _previousValue, int _newValue)
    {

        Debug.Log($"[{OwnerClientId}] randoemnumber : {_newValue} ");
    }

    void OnPlayerDataChanged(PlayerData _previousValue, PlayerData _newValue)
    {

        // Debug.Log($"[{OwnerClientId}] point : {_newValue.Point} , power : {_newValue.Power} , message : {_newValue.Message}");
        Debug.Log($" message : {_newValue.Message}");
    }

    void Update()
    {

        if (!IsOwner || !IsSpawned) return;


        if (Input.GetKeyDown(KeyCode.T))
        {
            CreateAvatarServerRpc();
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {

            var point = UnityEngine.Random.Range(0, 100);
            var power = UnityEngine.Random.Range(0, 100);
            playerData.Value = new PlayerData { Point = point, Power = power, Message = $"Message from - {OwnerClientId}" };

        }


        // Vector3 moveDir = Vector3.zero;

        // if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        // if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        // if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        // if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        // float speed = 3f;
        // transform.position += moveDir * speed * Time.deltaTime;
    }


    [ServerRpc]
    private void CreateAvatarServerRpc()
    {
        var trans = Instantiate(m_TestObjectPrefab);
        trans.gameObject.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId, true);


    }


}
