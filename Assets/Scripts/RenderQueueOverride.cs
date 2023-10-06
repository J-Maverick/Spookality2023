
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RenderQueueOverride : UdonSharpBehaviour
{
    public Material[] materials;

    void Start() {
        foreach (Material material in materials) {
            material.renderQueue = 2450;
        }
    }
}
