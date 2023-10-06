
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class RemoveButton : UdonSharpBehaviour
{
    public BubblePool pool;

    public override void Interact()
    {
        pool.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.Owner, "RemoveBubbles");
    }
}
