
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpectatorCam : UdonSharpBehaviour
{
    bool toggledActive = false;
    public bool localPlayerInZone = false;
    public GameObject spectatorCam;

    public override void Interact()
    {
        toggledActive = !toggledActive;
        SetState();
    }

    public void SetState() {
        if (toggledActive && localPlayerInZone) {
            spectatorCam.SetActive(true);
        }
        else {
            spectatorCam.SetActive(false);
        }
    }
}
