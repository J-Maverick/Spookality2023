
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorkLights : UdonSharpBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);
    }
}
