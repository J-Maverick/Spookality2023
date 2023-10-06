
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
    void Start()
    {
        localPlayer = Networking.LocalPlayer;
        SetSizeModifier();
    }


    public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float prevEyeHeightAsMeters)
    {
        if (player.isLocal) {
            SetSizeModifier();
        }
    }

    public void SetSizeModifier() {
        if (sizeModEnabled) {
            sizeModifier = localPlayer.GetAvatarEyeHeightAsMeters() / defaultSize;
        }
        else {
            sizeModifier = 1f;
        }
        SetMoveSpeed();
        SetJumpImpulse();
    }

    public void SetMoveSpeed()
    {
        localPlayer.SetRunSpeed(defaultRunSpeed * sizeModifier);
        localPlayer.SetWalkSpeed(defaultWalkSpeed * sizeModifier);
        localPlayer.SetStrafeSpeed(defaultStrafeSpeed * sizeModifier);
    }

    public void SetJumpImpulse()
    {
        localPlayer.SetJumpImpulse(defaultJumpImpulse);
    }

}
