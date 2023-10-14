
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SpectatorCamTrigger : UdonSharpBehaviour
{
    public SpectatorCam cam;

    public override void OnPlayerTriggerEnter(VRCPlayerApi player)
    {
        if (player.IsValid() && player.isLocal) {
            cam.localPlayerInZone = true;
            cam.SetState();
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.IsValid() && player.isLocal) {
            cam.localPlayerInZone = false;
            cam.SetState();
        }
    }
}
