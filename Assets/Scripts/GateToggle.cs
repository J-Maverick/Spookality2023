
using UdonSharp;
using Unity.Mathematics;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GateToggle : UdonSharpBehaviour
{
    public bool open = false;
    public bool fullyOpen = false;
    public bool fullyClosed = false;
    public float openRate = 0.00001f;
    public float openRateMultiplier = 1.1f;
    public float closeRate = 2f;
    float _openRate = 0f;
    public GameManager gameManager;
    public GateManager gateManager;
    public AudioSource openSound;
    public AudioSource closeSound;
    public Quaternion openRotation = Quaternion.Euler(90f, 0f, 0f);
    public Quaternion closeRotation = Quaternion.identity;

    public override void Interact()
    {
        if (gameManager.localPlayerType == LocalPlayerType.INNOCENT_FREE && gateManager.CanOpenNew()) { 
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Open");
        }
        else if (gameManager.localPlayerType == LocalPlayerType.HUNTER) { 
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Close");
        }
    }
    public void Open()
    {
        if (!open && gateManager.CanOpenNew()) {  
            _openRate = openRate;
            open = true;
            fullyClosed = false;
            openSound.Play();
        }
    }

    public void Close() {
        openSound.Stop();
        if (open) {
            closeSound.Play();
        }
        _openRate = openRate;
        open = false;
        fullyOpen = false;
        gateManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShutAllGates");
    }

    void FixedUpdate() {
        if (open) {
            if (!fullyOpen) {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, openRotation, _openRate);
                _openRate *= openRateMultiplier;
                if (transform.localRotation == openRotation) {
                    fullyOpen = true;
                }
            }
        }
        else {
            if (!fullyClosed) {
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, closeRotation, closeRate);
                if (transform.localRotation == closeRotation) {
                    fullyClosed = true;
                }
            }
        }
    }

}
