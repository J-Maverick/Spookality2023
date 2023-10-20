
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HunterItems : UdonSharpBehaviour
{
    public LightsOnHead headLight;
    public SmokeBody smokeBody;
    public bool hunterAssigned = false;
    public Animator animator;
    public GameManager gameManager;

    public void AssignHunter(VRCPlayerApi player) {
        if (player != null && player.IsValid()) {
            Networking.SetOwner(player, gameObject);
            Networking.SetOwner(player, headLight.gameObject);
            Networking.SetOwner(player, smokeBody.gameObject);
            hunterAssigned = true;
            Debug.LogFormat("{0}: Assigned to hunter {1}[{2}]", name, player.displayName, player.playerId);
        }
    }

    public void ActivateItems() {
        Debug.LogFormat("{0}: Attempting to activate", name);
        if (gameManager.hunters.Contains(Networking.GetOwner(gameObject).playerId)) {
            headLight.active = true;
            smokeBody.active = true;
            Debug.LogFormat("{0}: Activated", name);  
        }
        else {
            Debug.LogFormat("{0}: Attempted to activate on non-hunter, deactivating.", name);
            DeactivateItems();
        }
    }

    public void DeactivateItems() {
        headLight.active = false;
        smokeBody.active = false;
        hunterAssigned = false;
        Debug.LogFormat("{0}: Deactivated", name);   
    }

    public void EnablePotato() {
        animator.SetBool("Potato", true);
    }

    public void DisablePotato() {
        animator.SetBool("Potato", false);
    }
}
