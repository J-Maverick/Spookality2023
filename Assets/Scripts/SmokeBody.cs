
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SmokeBody : UdonSharpBehaviour
{
    public GameObject particles = null;

    void Update()
    {
        if (Networking.LocalPlayer.isMaster) {
            particles.SetActive(false);
        }
        else {
            transform.position = Networking.GetOwner(gameObject).GetPosition();
        }
    } 
            
}
