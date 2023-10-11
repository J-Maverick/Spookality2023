
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
    public float timer = 0f;

    public void ShutAllGates() {
        foreach (GateToggle gate in gates) {
            gate.Close();
        }
    }

    public bool CanOpenNew()
    {
        if (timer > 0f) {
            return false;
        }
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
        if (gameManager.gameInProgress) {
            if (AllGatesClosed()) {
                if (player.isLocal && gameManager.localPlayerType != LocalPlayerType.HUNTER) {
                    gameManager.localPlayerType = LocalPlayerType.INNOCENT_CAPTURED;
                }
                if (!containedPlayers.Contains(player.playerId)) {
                    containedPlayers.Add(player.playerId);
                }
            }
        }
        else {
            if (player.isLocal) {
                gameManager.SendPlayerHome();
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (!AllGatesClosed()) {
            if (player.isLocal && gameManager.players.Contains(player.playerId) && gameManager.localPlayerType != LocalPlayerType.HUNTER) {
                gameManager.localPlayerType = LocalPlayerType.INNOCENT_FREE;
            }
            if (containedPlayers.Contains(player.playerId)) {
                containedPlayers.Remove(player.playerId);
            }
        }
        else if (gameManager.gameInProgress) {
            if (player.isLocal && gameManager.localPlayerType == LocalPlayerType.INNOCENT_CAPTURED) {
                player.TeleportTo(transform.position, transform.rotation);
            }
        }
    }

    public bool CheckVictory() {
        if (AllGatesClosed()) {
            for (int i=0; i < gameManager.players.Count; i++) {

                if (gameManager.hunters.Contains(gameManager.players[i])) continue;

                if (VRCPlayerApi.GetPlayerById(gameManager.players[i].Int) != null) {
                    if (!containedPlayers.Contains(gameManager.players[i])) {
                        return false;
                    }
                }
            }
            Debug.LogFormat("{0}: Victory condition reached! n contained players: {1}", name, containedPlayers.Count);
            return true;
        }
        return false;
    }

    public void ResetTimer() {
        timer = gameManager.gateCooldownTimeSecs;
    }

    void Update() {
        if (gameManager.gameInProgress) {
            if (Networking.LocalPlayer.isMaster) {
                gameManager.VictoryCondition(CheckVictory());
            }
            if (!AllGatesClosed() && containedPlayers.Count > 0) {
                containedPlayers.Clear();
            }
        }
        if (timer > 0f) {
            timer -= Time.deltaTime;
        }
    }
}
