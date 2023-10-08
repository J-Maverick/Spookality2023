
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EggOnHead : UdonSharpBehaviour
{

    VRCPlayerApi localPlayer = null;
    public bool active = false;
    public Transform bigEgg = null;

    void Start() {
        localPlayer = Networking.LocalPlayer;
    }

    void Update()
    {
        if (localPlayer != null && active) {
            transform.position = localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            if (bigEgg != null) {
                bigEgg.position = localPlayer.GetPosition();
            }
        }
        else {
            transform.position = Vector3.zero;
            if (bigEgg != null) {
                bigEgg.position = Vector3.zero;
            }
        }

    } 
}
