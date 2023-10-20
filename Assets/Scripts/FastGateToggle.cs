
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class FastGateToggle : UdonSharpBehaviour
{
    public bool masterOnly;
    public Toggle toggle;
    public GateManager gateManager;
    [UdonSynced] public bool value = true;

    void Start()
    {
        if (!Networking.LocalPlayer.isMaster) {
            toggle.interactable = false;
        }
    }

    public override void OnDeserialization()
    {
        toggle.isOn = value;
    }

    public void CallBack() {
        gateManager.fastMode = toggle.isOn;
        value = toggle.isOn;
    }
}
