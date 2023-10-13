
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
    public GateManager gateManager;
    public GameManager gameManager;

    public void Start() {
        _usingPlayer = Networking.GetOwner(gameObject);
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        _usingPlayer = player;
    }

    public override void OnPlayerJoined(VRCPlayerApi player)
    {
        if (player.isLocal) {
            _usingPlayer = Networking.GetOwner(gameObject);
        }
    }


    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (player != null && player == _usingPlayer) {
            _usingPlayer = null;
            Deactivate();
            bubblePool.RemoveBubble(gameObject);
        }
    }

    public void Activate() {
        _usingPlayer = Networking.GetOwner(gameObject);
        if (gameManager.players.Contains(_usingPlayer.playerId)) {
            if (!gameManager.hunters.Contains(_usingPlayer.playerId)) {
                bubbleActive = true;
                if (gameManager.localPlayerType == LocalPlayerType.HUNTER) {
                    bubbleCollider.enabled = true;
                }
                else {
                    bubbleCollider.enabled = false;
                }
            }
            else {
                bubbleActive = false;
                bubbleCollider.enabled = false;
            }
        }
        else {
            bubbleActive = false;
            bubbleCollider.enabled = false;
        }
        Debug.LogFormat("{0}: Activated | _usingPlayer: {1}[{2}] | localPlayerType: {3} | Collider: {4}", name, _usingPlayer.displayName, _usingPlayer.playerId, gameManager.localPlayerType.ToString(), bubbleCollider.enabled);
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
        if (gameManager.gameInProgress) {
            transform.position = _usingPlayer.GetPosition();
        }
        else if (bubbleActive) {
            Debug.LogFormat("{0}: This would have disabled the bubble!", name);
            // Deactivate();
        }
    }

    public void LogInfrequent(string log) {
        if (Time.frameCount % 500 == 0) {
            Debug.Log(log);
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

    public void TransferOwner(VRCPlayerApi player) {
        _usingPlayer = player;
        Networking.SetOwner(_usingPlayer, gameObject);
    }
}
