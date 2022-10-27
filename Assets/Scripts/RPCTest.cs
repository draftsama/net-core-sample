using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RPCTest : NetworkBehaviour
{




    void Start()
    {

    }

    public override void OnNetworkSpawn()
    {
        name = "RPC_" + OwnerClientId.ToString();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))
        {

            var targetClientID = new List<ulong> { 1, 2 };

            TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = targetClientID } });
        }

    }


    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log($"{name}");

    }
}
