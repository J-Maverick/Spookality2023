
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EggOnHead : UdonSharpBehaviour
{

    VRCPlayerApi localPlayer = null;
    public bool active = false;

    void Start() {
        localPlayer = Networking.LocalPlayer;
    }
    void Update()
    {
        if (localPlayer != null && active) {
            transform.position = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        }
        else {
            transform.position = Vector3.zero;
        }

    } 
}
