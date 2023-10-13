
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;

public class BubblePool : UdonSharpBehaviour
{
    public VRCObjectPool pool;
    public PlayerKillBubble[] bubbles;

    public GameManager gameManager;
    public EggOnHead egg;


    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        GameObject bubble = pool.TryToSpawn();
        if (bubble != null) {
            Debug.LogFormat("{0}: Successfully spawned bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
            bubble.GetComponent<PlayerKillBubble>().TransferOwner(player);
        }
    }

    public void ActivateBubbles() {
        foreach (PlayerKillBubble bubble in bubbles) {
            if (bubble._usingPlayer != null && gameManager.players.Contains(bubble._usingPlayer.playerId)) {
                bubble.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Activate");
            }
        }
    }

    public void DeactivateBubbles() {
        foreach (PlayerKillBubble bubble in bubbles) {
            bubble.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Deactivate");
        }
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
