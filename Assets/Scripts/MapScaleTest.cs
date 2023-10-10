
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class MapScaleTest : UdonSharpBehaviour
{
    void Start()
    {
        transform.localScale = Vector3.one * 1.75f;
        Networking.LocalPlayer.Respawn();
    }
}
