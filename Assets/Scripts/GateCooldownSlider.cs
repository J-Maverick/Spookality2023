
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class GateCooldownSlider : SettingsSlider
{
    public override void CallBack()
    {
        base.CallBack();
        gameManager.gateCooldownTimeSecs = value;
    }
}
