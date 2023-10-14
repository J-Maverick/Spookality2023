﻿
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
    [UdonSynced] public bool value = false;

    void Start()
    {
        if (!Networking.LocalPlayer.isMaster) {
            toggle.interactable = false;
        }
        else {
            value = toggle.isOn;
        }
    }

    public void CallBack() {
        gateManager.fastMode = toggle.isOn;
        value = toggle.isOn;
    }
}
