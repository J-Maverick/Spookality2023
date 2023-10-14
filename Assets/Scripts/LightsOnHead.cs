
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LightsOnHead : UdonSharpBehaviour
{
    public GameObject particles = null;
    [UdonSynced] public bool active = false;
    public VRCPlayerApi owner = null;

    void Start() {
        owner = Networking.GetOwner(gameObject);
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        owner = player;
    }

    void Update()
    {
        if (active && owner != null && owner.IsValid()) {
            if (owner.isLocal) {
                transform.position = owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                transform.rotation = owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;
                particles.SetActive(false);
            }
            else {      
                particles.SetActive(true);
                transform.position = owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                transform.rotation = owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

                // Account for some heads randomly flipping the tracking data... wow what an insane failure mode -- consider moving this logic to an OnAvatarChanged method
                Vector3 headAngles = owner.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation.eulerAngles;
                Vector3 bodyAngles = owner.GetRotation().eulerAngles;
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
