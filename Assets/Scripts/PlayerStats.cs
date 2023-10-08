
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerStats : UdonSharpBehaviour
{
    public float defaultWalkSpeed = 2f;
    public float defaultRunSpeed = 4f;
    public float defaultStrafeSpeed = 2f;
    public float defaultJumpImpulse = 3f;

    VRCPlayerApi localPlayer;
    public bool sizeModEnabled = true;
    public float sizeModifier = 1f;
    public float defaultSize = 1.84f;

    public float bigWalkSpeed = 3.33f;
    public float bigRunSpeed = 6.66f;
    public float bigStrafeSpeed = 3.33f;

    public float smallWalkSpeed = 1.1f;
    public float smallRunSpeed = 2.2f;
    public float smallStrafeSpeed = 1.1f;
    
    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        SetDefaultSpeed();
        SetJumpImpulse();
        // SetSizeModifier();
    }


    // public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
    // {
    //     if (player.isLocal) {
    //         SetSizeModifier();
    //     }
    // }

    // public void SetSizeModifier() {
    //     if (sizeModEnabled) {
    //         sizeModifier = localPlayer.GetAvatarEyeHeightAsMeters() / defaultSize;
    //         if (sizeModifier >= 0.9f) {
    //             SetBigSpeed();
    //         }
    //         else {
    //             SetSmallSpeed();
    //         }
    //     }
    //     else {
    //         sizeModifier = 1f;
    //         SetMoveSpeed();
    //     }
    //     SetJumpImpulse();
    // }

    
    public void SetBigSpeed()
    {
        localPlayer.SetRunSpeed(bigRunSpeed);
        localPlayer.SetWalkSpeed(bigWalkSpeed);
        localPlayer.SetStrafeSpeed(bigStrafeSpeed);
        Debug.LogFormat("{0}: Local Player Stats: RunSpeed: {1} | WalkSpeed: {2} | StrafeSpeed: {3}", name, localPlayer.GetRunSpeed(), localPlayer.GetWalkSpeed(), localPlayer.GetStrafeSpeed());
    }
    public void SetSmallSpeed()
    {
        localPlayer.SetRunSpeed(smallRunSpeed);
        localPlayer.SetWalkSpeed(smallWalkSpeed);
        localPlayer.SetStrafeSpeed(smallStrafeSpeed);
        Debug.LogFormat("{0}: Local Player Stats: RunSpeed: {1} | WalkSpeed: {2} | StrafeSpeed: {3}", name, localPlayer.GetRunSpeed(), localPlayer.GetWalkSpeed(), localPlayer.GetStrafeSpeed());
    }

    public void SetDefaultSpeed()
    {
        localPlayer.SetRunSpeed(defaultRunSpeed);
        localPlayer.SetWalkSpeed(defaultWalkSpeed);
        localPlayer.SetStrafeSpeed(defaultStrafeSpeed);
        Debug.LogFormat("{0}: Local Player Stats: RunSpeed: {1} | WalkSpeed: {2} | StrafeSpeed: {3}", name, localPlayer.GetRunSpeed(), localPlayer.GetWalkSpeed(), localPlayer.GetStrafeSpeed());
    }

    public void SetJumpImpulse()
    {
        localPlayer.SetJumpImpulse(defaultJumpImpulse);
    }

}
