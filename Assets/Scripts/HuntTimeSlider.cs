
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class HuntTimeSlider : SettingsSlider
{
    public override void CallBack()
    {
        base.CallBack();
        gameManager.gameLengthMin = value;
    }
}
