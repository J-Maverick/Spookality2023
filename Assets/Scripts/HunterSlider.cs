
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HunterSlider : SettingsSlider
{
    public override void CallBack()
    {
        base.CallBack();
        gameManager.nHunters = value;
    }
}
