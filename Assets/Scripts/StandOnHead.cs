
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StandOnHead : UdonSharpBehaviour
{
    BoxCollider boxCollider;
    VRCPlayerApi localPlayer = null;
    bool initialized = false;

    public void Start() {
        boxCollider = GetComponent<BoxCollider>();
        localPlayer = Networking.LocalPlayer;
        initialized = true;
    }
    public void Update()
    {
        if (initialized && transform.position.y - localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position.y > 0) {
            boxCollider.enabled = false;
        }
        else {
            boxCollider.enabled = true;
        }
    }
}
