
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorkLights : UdonSharpBehaviour
{
    public Animator animator;

    public void LightsOn() {
        animator.SetBool("LightsOn", true);
    }

    public void LightsOff() {
        animator.SetBool("LightsOn", false);
    }
}
