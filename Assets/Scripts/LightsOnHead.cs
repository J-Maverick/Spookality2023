
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LightsOnHead : UdonSharpBehaviour
{
    public GameObject particles = null;

    void Update()
    {
        if (Networking.LocalPlayer.isMaster) {
            transform.position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            transform.rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            particles.SetActive(false);
        }
        else {
            transform.position = Networking.GetOwner(gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            transform.rotation = Networking.GetOwner(gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
            transform.RotateAround(transform.position, transform.up, 180f);
        }
    } 
}
