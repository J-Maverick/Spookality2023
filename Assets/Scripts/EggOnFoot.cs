
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class EggOnFoot : UdonSharpBehaviour
{
    VRCPlayerApi localPlayer = null;

    void Start() {
        localPlayer = Networking.LocalPlayer;
    }

    void Update()
    {
        if (localPlayer.IsValid()) {
            transform.position = localPlayer.GetPosition();
        }
    } 
}
