
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class SettingsSlider : UdonSharpBehaviour
{
    public bool masterOnly = true;
    public Slider slider;
    public GameManager gameManager;
    public Text text;
    public String suffix = "";
    [UdonSynced] public int value = 0;

    void Start()
    {
        if (!Networking.LocalPlayer.isMaster) {
            slider.interactable = false;
        }
        else {
            value = (int) slider.value;
        }
    }

    public override void OnDeserialization()
    {
        slider.value = value;
        UpdateText();
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        if (player.isLocal && player.isMaster) {
            slider.interactable = true;
        }
    }

    public virtual void CallBack() {
        if (!gameManager.gameInProgress) {
            value = (int) slider.value;
        }
        else {
            slider.value = value;
        }
        UpdateText();
    }

    public void UpdateText() {
        text.text = string.Format("{0}{1}", value, suffix);
    }
}
