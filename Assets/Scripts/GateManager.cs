
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.SDKBase.Midi;
using VRC.Udon;

public class GateManager : UdonSharpBehaviour
{
    public GateToggle[] gates;
    public GameManager gameManager;
    public DataList containedPlayers = new DataList(){};

    public void ShutAllGates() {
        foreach (GateToggle gate in gates) {
            gate.Close();
        }
    }

    public bool CanOpenNew()
    {
        foreach (GateToggle gate in gates) {
            if (gate.open) {
                return false;
            }
        }
        return true;
    }

    public bool AllGatesClosed() {
        foreach (GateToggle gate in gates) {
            if (!gate.fullyClosed) {
                return false;
            }
        }
        return true;
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (player.isLocal) {
            gameManager.localPlayerType = LocalPlayerType.INNOCENT_CAPTURED;
        }
        if (!containedPlayers.Contains(player.playerId)) {
            containedPlayers.Add(player.playerId);
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (player.isLocal) {
            gameManager.localPlayerType = LocalPlayerType.INNOCENT_FREE;
        }
        if (containedPlayers.Contains(player.playerId)) {
            containedPlayers.Remove(player.playerId);
        }
    }

    public bool CheckVictory() {
        for (int i=0; i < gameManager.players.Count; i++) {

            if (gameManager.hunters.Contains(gameManager.players[i])) continue;

            if (VRCPlayerApi.GetPlayerById(gameManager.players[i].Int).IsValid()) {
                if (!containedPlayers.Contains(gameManager.players[i])) {
                    return false;
                }
            }
        }
        return true;
    }

    void Update() {
        // if (Networking.LocalPlayer.isMaster) {
        //     gameManager.VictoryCondition(CheckVictory());
        // }
    }
}
