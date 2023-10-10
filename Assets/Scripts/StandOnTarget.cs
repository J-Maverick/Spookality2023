
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StandOnTarget : StandOnHead
{
    public Transform target;

    public override void Update()
    {
        if (logicActive) {
            if (target.position.y - localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position.y > 0) {
                boxCollider.enabled = false;
            }
            else {
                boxCollider.enabled = true;
            }
        }
    }
}
