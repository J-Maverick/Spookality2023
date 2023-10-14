
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GrainByDistance : UdonSharpBehaviour
{
    public GameManager gameManager;
    public Animator animator;
    public float maxDistance = 10f;
    public float distance = 10f;

    void Update() {
        if (gameManager.gameInProgress && (gameManager.localPlayerType == LocalPlayerType.INNOCENT_FREE || gameManager.localPlayerType == LocalPlayerType.INNOCENT_CAPTURED)) {
            distance = maxDistance;
            Vector3 localPlayerPosition = Networking.LocalPlayer.GetPosition();
            for (int i=0; i<gameManager.hunters.Count; i++) {
                if (VRCPlayerApi.GetPlayerById(gameManager.hunters[i].Int).IsValid()) {
                    float distBetween = Vector3.Distance(VRCPlayerApi.GetPlayerById(gameManager.hunters[i].Int).GetPosition(), localPlayerPosition);
                    if (distBetween < distance) {
                        distance = distBetween;
                    }
                }
            }
            animator.SetFloat("Grain", 1f - (distance / maxDistance));
        }
        else {
            animator.SetFloat("Grain", 0f);
        }
    }
}
