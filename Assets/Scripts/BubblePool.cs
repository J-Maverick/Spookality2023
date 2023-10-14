
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Components;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms;
using BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto;

public class BubblePool : UdonSharpBehaviour
{
    // public VRCObjectPool pool;
    public PlayerKillBubble[] bubbles;

    public GameManager gameManager;
    public EggOnHead egg;

    private int nRetries = 10;

    // public override void OnPlayerJoined(VRCPlayerApi player)
    // {
    //     GameObject bubble = pool.TryToSpawn();
    //     if (bubble != null) {
    //         Debug.LogFormat("{0}: Successfully spawned bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
    //         bubble.GetComponent<PlayerKillBubble>().TransferOwner(player);
    //     }
    //     else {
    //         for (int i = 0; i < nRetries; i++) {
    //             Debug.LogFormat("{0}: Failed to spawn bubble for {1}[{2}], retrying... {3}/{4} retries.", name, player.displayName, player.playerId, i + 1, nRetries);
    //             bubble = pool.TryToSpawn();
    //             if (bubble != null) {
    //                 Debug.LogFormat("{0}: Successfully spawned bubble for player: {1}[{2}]", name, player.displayName, player.playerId);
    //                 bubble.GetComponent<PlayerKillBubble>().TransferOwner(player);
    //                 break;
    //             }
    //         }
    //     }
    // }

    public void ActivateBubbles() {
        // foreach (PlayerKillBubble bubble in bubbles) {
        //     if (bubble.gameObject.activeSelf) {
        //         bool goodbubble = bubble._usingPlayer != null && gameManager.players.Contains(bubble._usingPlayer.playerId);
        //         Debug.LogFormat("{0} Trying to activate {1} | _usingPlayer: {2} | goodBubble: {3} | gameObject.Active: {4}", name, bubble.name, bubble._usingPlayer, goodbubble, bubble.gameObject.activeSelf);
        //         if (goodbubble) {
        //             // bubble.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Activate");
        //             bubble.Activate();
        //         }
        //     }
        //     else {
        //         Debug.LogFormat("{0}: Tried activating {1}... object is not active.", name, bubble.name);
        //     }
        // }
    }

    public void DeactivateBubbles() {
        // foreach (PlayerKillBubble bubble in bubbles) {
        //     bubble.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Deactivate");
        // }
    }

    public void ActiveEgg() {
        egg.active = true;
    }

    public void InactiveEgg() {
        egg.active = false;
    }

    // public void RemoveBubble(GameObject bubble) {
    //     pool.Return(bubble);
    // }
}
