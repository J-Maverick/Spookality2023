
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class TaterToggle : UdonSharpBehaviour
{
    public PotatoZone potatoZone;
    public void ToggleTaters()
    {
        potatoZone.UpdateStateFromToggles();
    }
}
