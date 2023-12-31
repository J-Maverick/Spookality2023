﻿
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
    
    [UdonSynced, FieldChangeCallback(nameof(fastMode))]
    private bool _fastMode = true;
    public bool fastMode
    {
        set
        {
            _fastMode = value;
            if (_fastMode) {
                foreach (GateToggle gate in gates) {
                    gate.openRate = gate.openRateFast;
                    gate.openRateMultiplier = gate.openRateMultiplierFast;
                    gate.openSound = gate.openSoundFast;
                }
            }
            else {
                foreach (GateToggle gate in gates) {
                    gate.openRate = gate.openRateSlow;
                    gate.openRateMultiplier = gate.openRateMultiplierSlow;
                    gate.openSound = gate.openSoundSlow;
                }
            }
        }
        get => _fastMode;
    }

    public bool gbjEscapeCheck = false;

    public void ShutAllGates() {
        if (!AllGatesClosed() && gameManager.localPlayerType == LocalPlayerType.HUNTER) {
            SendCustomEventDelayedSeconds("ClearContainedPlayers", 2f);
        }
        foreach (GateToggle gate in gates) {
            gate.Close();
        }
    }

    public void ClearContainedPlayers() {
        containedPlayers.Clear();
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
                    Debug.LogFormat("{0}: player added to containment: {1}[{2}]", name, player.displayName, player.playerId);
                }
            }
            gbjEscapeCheck = false;
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
                Debug.LogFormat("{0}: player removed from containment: {1}[{2}]", name, player.displayName, player.playerId);
            }
        }
        else if (gameManager.gameInProgress) {
            if (player.isLocal && gameManager.localPlayerType == LocalPlayerType.INNOCENT_CAPTURED) {
                player.TeleportTo(transform.position, transform.rotation);
                SendCustomEventDelayedSeconds("GBJEscapeGatherer", 2f);
            }
        }
        gbjEscapeCheck = true;
    }

    public void GBJEscapeGatherer() {
        if (gbjEscapeCheck && containedPlayers.Contains(Networking.LocalPlayer.playerId) && gameManager.localPlayerType != LocalPlayerType.HUNTER) {
            Networking.LocalPlayer.TeleportTo(transform.position, transform.rotation);
            SendCustomEventDelayedSeconds("GBJEscapeGatherer", 2f);
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
                Debug.LogFormat("{0}: Cleared containment list", name);
            }
        }
        if (timer > 0f) {
            timer -= Time.deltaTime;
        }
    }
}
