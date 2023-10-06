
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using VRC.SDK3.Data;

public class BubblePool : UdonSharpBehaviour
{
    public VRCObjectPool pool;
    public BoxCollider boxCollider;
    public Text text;

    public DataList playerList = new DataList(){};
    public DataToken[] players;
    public bool localPlayerInZone = false;
    public EggOnHead egg;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.isLocal) {
            localPlayerInZone = true;
        }
        playerList.Add(player.playerId);
        players = playerList.ToArray();
        UpdateText();
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal) {
            localPlayerInZone = false;
        }
        playerList.Remove(player.playerId);
        players = playerList.ToArray();
        UpdateText();
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (player != null && playerList.Contains(player.playerId)) {
            playerList.Remove(player.playerId);
            players = playerList.ToArray();
            UpdateText();
        }
    }

    public void UpdateText() {
        text.text = string.Format("Players: \n{0}", players.Length);
    }


    // public override void OnPlayerJoined(VRCPlayerApi player)
    // {
    //     if (Networking.LocalPlayer.IsOwner(gameObject)) {
    //         Debug.LogFormat("{0}: Spawning bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
    //         GameObject bubble = pool.TryToSpawn();
    //         if (bubble != null) {
    //             Debug.LogFormat("{0}: Successfully spawned bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
                
    //             bubble.GetComponent<PlayerKillBubble>().TransferOwner(player);
    //         }
    //     }
    // }

    public void AssignBubbles() {
        RemoveBubbles();

        foreach (int id in players) {
            VRCPlayerApi player = VRCPlayerApi.GetPlayerById(id);
            if (player != null) {
                Debug.LogFormat("{0}: Spawning bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
                GameObject bubble = pool.TryToSpawn();
                if (bubble != null) {
                    Debug.LogFormat("{0}: Successfully spawned bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
                    bubble.GetComponent<PlayerKillBubble>().TransferOwner(player);
                }
            }
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ActiveEgg");
    }

    public void RemoveBubbles() {
        foreach (GameObject bubble in pool.Pool) {
            bubble.GetComponent<PlayerKillBubble>()._usingPlayer = null;
            RemoveBubble(bubble);
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InactiveEgg");
    }

    public void ActiveEgg() {
        egg.active = true;
    }

    public void InactiveEgg() {
        egg.active = false;
    }

    public void RemoveBubble(GameObject bubble) {
        pool.Return(bubble);
    }
}
