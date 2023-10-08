
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class FlashlightSlider : SettingsSlider
{
    public override void CallBack()
    {
        base.CallBack();
        gameManager.nFlashlights = value;
    }
}
