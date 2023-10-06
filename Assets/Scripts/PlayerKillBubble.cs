
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
    public BubblePool bubblePool = null;

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
        if (!_usingPlayer.isLocal) {
            bubbleCollider.enabled = true;
        }
        Debug.LogFormat("{0}: Initialized... _usingPlayer: {1}[{2}]", name, _usingPlayer.displayName, _usingPlayer.playerId);
    }

    public void Deactivate() {
        bubbleActive = false;
        bubbleCollider.enabled = false;
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "PlayCapture");
        PlayCaptureSound();
    }

    void Update() {
        if (_usingPlayer != null) {
            transform.position = _usingPlayer.GetPosition();
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
