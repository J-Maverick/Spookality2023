
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
    public float _openRate = 0f;
    public GameManager gameManager;
    public GateManager gateManager;
    public AudioSource openSound;
    public AudioSource closeSound;
    public Quaternion openRotation = Quaternion.Euler(90f, 0f, 0f);
    public Quaternion closeRotation = Quaternion.identity;
    public Collider selectionCollider;

    public override void Interact()
    {
        if (gameManager.localPlayerType == LocalPlayerType.INNOCENT_FREE && gateManager.CanOpenNew()) { 
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Open");
        }
        else if (gameManager.localPlayerType == LocalPlayerType.HUNTER) { 
            gateManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShutAllGates");
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
            gateManager.ResetTimer();
        }
        _openRate = openRate;
        open = false;
        fullyOpen = false;
    }

    public void Update() {
        if (gameManager.gameInProgress) {
            if (gameManager.localPlayerType == LocalPlayerType.INNOCENT_FREE && gateManager.CanOpenNew()) { 
                selectionCollider.enabled = true;
            }
            else if (gameManager.localPlayerType == LocalPlayerType.HUNTER && open) { 
                selectionCollider.enabled = true;
            }
            else {
                selectionCollider.enabled = false;
            }
        }
    }

    public void FixedUpdate() {
        if (open) {
            if (!fullyOpen) {
                Debug.LogFormat("{0}: FixedUpdate: opening", name);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, openRotation, _openRate * Time.fixedDeltaTime);
                _openRate += _openRate * openRateMultiplier * Time.fixedDeltaTime;
                if (transform.localRotation == openRotation) {
                    fullyOpen = true;
                }
            }
        }
        else {
            if (!fullyClosed) {
                Debug.LogFormat("{0}: FixedUpdate: closing", name);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, closeRotation, closeRate * Time.fixedDeltaTime);
                if (transform.localRotation == closeRotation) {
                    fullyClosed = true;
                }
            }
        }
    }

}
