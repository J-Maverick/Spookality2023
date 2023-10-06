using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ChairOnButt : UdonSharpBehaviour
{
    public VRCStation station;
    public VRC_Pickup pickup;
    public ChairsForButts chairsForButts;

    private VRCPlayerApi _usingPlayer;

    private bool tempExit = false;
    public bool trueLocal = false;

    public Rigidbody rb;

    public void _EnterStation()
    {
        station.PlayerMobility = VRC.SDKBase.VRCStation.Mobility.Mobile;
        VRCPlayerApi player = Networking.LocalPlayer;
        _UpdateLocal();
        station.UseStation(player);
    }

    public override void OnStationEntered(VRCPlayerApi player)
    {
        if (!tempExit) {
            _usingPlayer = player;
        }
    }

    public override void OnStationExited(VRCPlayerApi player)
    {
        if (!tempExit) {
            _usingPlayer = null;
            station.PlayerMobility = VRC.SDKBase.VRCStation.Mobility.ImmobilizeForVehicle;
            if (player.isLocal)
            {
                _UpdateLocal();
            }
        }
    }

    public override void OnOwnershipTransferred(VRCPlayerApi player)
    {
        Debug.LogFormat("{0}: OnOwnershipTransferred(Player: {1}[{2}])", name, player.displayName, player.playerId);
        if (_usingPlayer == null) {
            _usingPlayer = player;
            if (player.isLocal)
            {
                chairsForButts.OnLocalPlayerAssigned(this);
                pickup.pickupable = false;
            }
        }
    }

    public VRCPlayerApi GetUser()
    {
        return _usingPlayer;
    }

    private void Update()
    {
        if (!Utilities.IsValid(_usingPlayer))
        {
            return;
        }
        if (_usingPlayer.isLocal)
        {
            _UpdateLocal();
        }
    }

    private void _UpdateLocal()
    {
        if (pickup.IsHeld) {
            station.PlayerMobility = VRC.SDKBase.VRCStation.Mobility.ImmobilizeForVehicle;
            rb.isKinematic = false;
            tempExit = true;
        }
        else if (tempExit && rb.velocity.magnitude <= 0.001f && rb.angularVelocity.magnitude <= 0.001f) {
            Networking.SetOwner(_usingPlayer, gameObject);
        }
        else if (_usingPlayer != null && !tempExit) {
            station.PlayerMobility = VRC.SDKBase.VRCStation.Mobility.Mobile;
            rb.isKinematic = true;
            Vector3 pos = _usingPlayer.GetPosition();
            Quaternion rot = _usingPlayer.GetRotation();

            transform.SetPositionAndRotation(pos, rot);
        }
    }

}