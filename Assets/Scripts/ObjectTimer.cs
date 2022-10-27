using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ObjectTimer : NetworkBehaviour
{

    public override void OnNetworkSpawn()
    {
        name = "Object of " + OwnerClientId;
    }
    private void Start()
    {
        if (IsOwner)
            StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        yield return new WaitForSeconds(5f);
        DeSpawnServerRpc();
    }

    [ServerRpc]
    void DeSpawnServerRpc()
    {
        var net = GetComponent<NetworkObject>();
        if (net) net.Despawn(true);
    }
}
