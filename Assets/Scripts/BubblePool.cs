
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

public class BubblePool : UdonSharpBehaviour
{
    public VRCObjectPool pool;

    public GameManager gameManager;
    public EggOnHead egg;

    public void AssignBubble(VRCPlayerApi player) {
        if (player != null) {
            Debug.LogFormat("{0}: Spawning bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
            GameObject bubble = pool.TryToSpawn();
            if (bubble != null) {
                Debug.LogFormat("{0}: Successfully spawned bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
                bubble.GetComponent<PlayerKillBubble>().TransferOwner(player);
            }
        }
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
