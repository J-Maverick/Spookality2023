
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class SmokeBody : UdonSharpBehaviour
{
    public GameObject particles = null;
    [UdonSynced] public bool active = false;

    void Update()
    {
        if (active) {
            if (Networking.GetOwner(gameObject).isLocal) {
                particles.SetActive(false);
            }
            else {
                particles.SetActive(true);
                transform.position = Networking.GetOwner(gameObject).GetPosition();
            }
        }
        else {
            transform.position = Vector3.zero;
            particles.SetActive(false);
        }
    } 
            
}
