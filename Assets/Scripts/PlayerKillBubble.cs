
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerKillBubble : UdonSharpBehaviour
{
    public VRCPlayerApi _usingPlayer = null;
    public bool bubbleActive = false;
    public Collider bubbleCollider = null;
    public Transform gBJLocation = null;
    public PlayRandomSound soundFX = null;
    public PlayRandomSound screamFX = null;
    public BubblePool bubblePool = null;
    public float interactProximity = 5f;
    public GateManager gateManager;
    public GameManager gameManager;

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Activate");
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal) {
            _usingPlayer = Networking.GetOwner(gameObject);
            Activate();
        }
    }

    public void Activate() {
        _usingPlayer = Networking.GetOwner(gameObject);
        bubbleActive = true;
        if (gameManager.localPlayerType == LocalPlayerType.HUNTER) {
            bubbleCollider.enabled = true;
        }
        else {
            bubbleCollider.enabled = false;
        }
        Debug.LogFormat("{0}: Initialized | _usingPlayer: {1}[{2}] | localPlayerType: {3} | Collider: {4}", name, _usingPlayer.displayName, _usingPlayer.playerId, gameManager.localPlayerType.ToString(), bubbleCollider.enabled);
    }

    public void Deactivate() {
        bubbleActive = false;
        bubbleCollider.enabled = false;
        Debug.LogFormat("{0}: Deactivated.", name);
    }

    public override void Interact()
    {
        if (gameManager.localPlayerType == LocalPlayerType.HUNTER && !gateManager.containedPlayers.Contains(Networking.GetOwner(gameObject).playerId)) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "PlayCapture");
            gateManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShutAllGates");
            PlayCaptureSound();
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlayScream");
        }
    }

    void Update() {
        if (_usingPlayer != null) {
            transform.position = _usingPlayer.GetPosition();
            if (bubbleActive && gameManager.localPlayerType == LocalPlayerType.HUNTER) {
                if (!gateManager.containedPlayers.Contains(Networking.GetOwner(gameObject).playerId) && Vector3.Distance(transform.position, Networking.LocalPlayer.GetPosition()) < interactProximity) {
                    bubbleCollider.enabled = true;
                }
                else {
                    bubbleCollider.enabled = false;
                }
            }
        }
        else {
            _usingPlayer = Networking.GetOwner(gameObject);
            Debug.LogFormat("{0}: Caught empty _usingPlayer in update, filling with owner: {1}[{2}]", name, _usingPlayer.displayName, _usingPlayer.playerId);
            OnOwnershipTransferred(_usingPlayer);
        }
    }

    public void PlayCapture() {
        _usingPlayer.TeleportTo(gBJLocation.position, gBJLocation.rotation);
        PlayCaptureEffect();
        PlayCaptureSound();
    }

    public void PlayCaptureEffect() {

    }

    public void PlayScream() {
        if (!Networking.GetOwner(gameObject).isLocal && (gameManager.localPlayerType == LocalPlayerType.INNOCENT_FREE || gameManager.localPlayerType == LocalPlayerType.NON_PARTICIPANT)) {
            screamFX.Play();
        }
    }

    public void PlayCaptureSound() {
        soundFX.Play();
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (player != null && player == _usingPlayer) {
            _usingPlayer = null;
            bubblePool.RemoveBubble(gameObject);
        }
    }

    public void TransferOwner(VRCPlayerApi player) {
        _usingPlayer = player;
        if (player.isLocal) {  
            OnOwnershipTransferred(player);
        }
        else {
            Networking.SetOwner(_usingPlayer, gameObject);
        }
    }

}
