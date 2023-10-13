
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class MasterText : UdonSharpBehaviour
{
    public Text text;
    public void Start() {
        SetMasterText();
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        SetMasterText();
    }

    public void SetMasterText() {
        text.text = Networking.GetOwner(gameObject).displayName;
    }
}
