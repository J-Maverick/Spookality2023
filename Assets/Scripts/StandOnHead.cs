
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class StandOnHead : UdonSharpBehaviour
{
    public BoxCollider boxCollider;
    public VRCPlayerApi localPlayer = null;
    public bool logicActive = false;

    public void Start() {
        boxCollider = GetComponent<BoxCollider>();
        localPlayer = Networking.LocalPlayer;
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        logicActive = false;
        boxCollider.enabled = true;
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        logicActive = true;
    }

    public virtual void Update()
    {
        if (logicActive) {
            if (transform.position.y - localPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position.y > 0) {
                boxCollider.enabled = false;
            }
            else {
                boxCollider.enabled = true;
            }
        }
    }
}
