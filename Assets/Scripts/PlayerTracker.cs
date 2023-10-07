
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayerTracker : UdonSharpBehaviour
{
    VRCPlayerApi localPlayer;

    void Start()
    {
        localPlayer = Networking.LocalPlayer;
    }

    public void Update() {
        transform.SetPositionAndRotation(localPlayer.GetPosition(), localPlayer.GetRotation());
    }
}
