
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class KillBubbleTrigger : UdonSharpBehaviour
{
    public PlayerKillBubble bubble;

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        // if (player.isLocal && !bubble._usingPlayer.isLocal) {
        //     LogInfrequent(string.Format("{0}: Inside Trigger, bubble active state: {1} | local player is hunter: {2} | trigger player is local: {3}", name, bubble.bubbleActive, bubble.gameManager.localPlayerType == LocalPlayerType.HUNTER, player.isLocal));
        // }
        if (bubble.bubbleActive && bubble.gameManager.localPlayerType == LocalPlayerType.HUNTER && player.isLocal) {
            if (!bubble.gateManager.containedPlayers.Contains(bubble._usingPlayer.playerId)) {
                bubble.bubbleCollider.enabled = true;
                // LogInfrequent(string.Format("{0}: Bubble collider enabled inside trigger", name));
            }
            else {
                bubble.bubbleCollider.enabled = false;
                // LogInfrequent(string.Format("{0}: Bubble collider disabled inside trigger", name));
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        bubble.bubbleCollider.enabled = false;
    }


    public void LogInfrequent(string log) {
        if (Time.frameCount % 500 == 0) {
            Debug.Log(log);
        }
    }
}
