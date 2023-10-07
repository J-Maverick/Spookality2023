
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HunterItems : UdonSharpBehaviour
{
    public LightsOnHead headLight;
    public SmokeBody smokeBody;
    public bool hunterAssigned = false;

    public void AssignHunter(VRCPlayerApi player) {
        Networking.SetOwner(player, gameObject);
        Networking.SetOwner(player, headLight.gameObject);
        Networking.SetOwner(player, smokeBody.gameObject);
        hunterAssigned = true;
        Debug.LogFormat("{0}: Assigned to hunter {1}[{2}]", name, player.displayName, player.playerId);
    }

    public void ActivateItems() {
        Debug.LogFormat("{0}: Attempting to activate", name);
        if (hunterAssigned) { 
            headLight.active = true;
            smokeBody.active = true;
            Debug.LogFormat("{0}: Activated", name);  
        }
    }

    public void DeactivateItems() {
        headLight.active = false;
        smokeBody.active = false;
        hunterAssigned = false;
        Debug.LogFormat("{0}: Deactivated", name);   
    }
}
