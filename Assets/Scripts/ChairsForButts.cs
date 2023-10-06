
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Collections;
using VRC.SDK3.Components;

public class ChairsForButts : UdonSharpBehaviour
{
    public ChairOnButt playerStation;
    public bool stationAssigned = false;

    public VRCObjectPool objectPool;

    public void Initialize()
    {
        playerStation._EnterStation();
    }

    public override void OnPlayerRespawn(VRCPlayerApi player)
    {
        if (player.isLocal && stationAssigned)
        {
            playerStation._EnterStation();
        }
    }

    public void OnLocalPlayerAssigned(ChairOnButt station)
    {
        if (playerStation == null || (playerStation.GetUser() != null && !playerStation.GetUser().isLocal))
        {
            stationAssigned = true;
            playerStation = station;
            Initialize();
            Networking.LocalPlayer.Respawn();
            Debug.LogFormat("{0}: OnLocalPlayerAssigned Triggered!");
        }
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (Networking.LocalPlayer.isMaster)
        {
            GameObject spawnedStation = objectPool.TryToSpawn();
            Networking.SetOwner(player, spawnedStation);
            if (player.isLocal) spawnedStation.GetComponent<ChairOnButt>().OnOwnershipTransferred(player);
        }
    }

    public void ReturnStation(GameObject station)
    {
        if (Networking.GetOwner(objectPool.gameObject).isLocal)
        {
            Debug.LogFormat("{0}: Returning {1} to object pool.", name, station.name);
            objectPool.Return(station);
        }
    }
}