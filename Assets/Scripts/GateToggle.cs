
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GateToggle : UdonSharpBehaviour
{
    bool open = false;
    public float openRate = 0.00001f;
    public float openRateMultiplier = 1.1f;
    public float closeRate = 2f;
    float _openRate = 0f;
    public override void Interact()
    {
        ToggleGate();
    }
    void ToggleGate()
    {
        open = !open;
        _openRate = openRate;
    }

    void FixedUpdate() {
        if (open) {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.Euler(90f, 0f, 0f), _openRate);
            _openRate *= openRateMultiplier;
        }
        else {
            transform.localRotation = Quaternion.RotateTowards(transform.localRotation, Quaternion.identity, closeRate);
        }
    }

}
