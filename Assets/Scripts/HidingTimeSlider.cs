
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HidingTimeSlider : SettingsSlider
{
    public override void CallBack()
    {
        base.CallBack();
        gameManager.hidingTimeSecs = value;
    }
}
