
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LightsOnHead : UdonSharpBehaviour
{
    public GameObject particles = null;
    [UdonSynced] public bool active = false;

    void Update()
    {
        if (active) {
            if (Networking.GetOwner(gameObject).isLocal) {
                transform.position = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                transform.rotation = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
                particles.SetActive(false);
            }
            else {      
                particles.SetActive(true);
                transform.position = Networking.GetOwner(gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                transform.rotation = Networking.GetOwner(gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

                // Account for some heads randomly flipping the tracking data... wow what an insane failure mode -- consider moving this logic to an OnAvatarChanged method
                Vector3 headAngles = Networking.GetOwner(gameObject).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation.eulerAngles;
                Vector3 bodyAngles = Networking.GetOwner(gameObject).GetRotation().eulerAngles;
                if (170f < Mathf.Abs(headAngles.y - bodyAngles.y) && Mathf.Abs(headAngles.y - bodyAngles.y) < 190f) {  
                    transform.RotateAround(transform.position, transform.up, 180f);
                }
            }
        }
        else {
            transform.position = Vector3.zero;
            particles.SetActive(false);
        }
    } 
}
