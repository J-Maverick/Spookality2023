
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;
using VRC.SDK3.Components;


public enum LocalPlayerType {
    HUNTER = 0,
    INNOCENT_FREE = 1,
    INNOCENT_CAPTURED = 2,
    NON_PARTICIPANT = 3,
}

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class GameManager : UdonSharpBehaviour
{
    public BubblePool bubblePool;
    public bool localPlayerInZone = false;
    public Text text;
    public Text victorySplash;
    public PlayerTracker playerTracker;
    public AudioRamp ambience;
    public AudioSource innocentWinSound;
    public AudioSource hunterWinSound;

    public WorkLights workLights;
    public GateManager gateManager;

    [UdonSynced, FieldChangeCallback(nameof(gameInProgress))]
    private bool _gameInProgress;

    public bool gameInProgress
    {
        set
        {
            bool startUp = !_gameInProgress && value;
            _gameInProgress = value;
            if (startUp && Networking.LocalPlayer.isMaster) {
                StartGame();
            }
            else if (startUp) {
                SendCustomEventDelayedFrames("StartGame", 1);
            }
            Debug.LogFormat("{0}: Setting gameInProgress: {1}", _gameInProgress);
        }
        get => _gameInProgress;
    }
    [UdonSynced] public bool hidingPhase = false;
    [UdonSynced] public int gameLengthMin = 5;
    [UdonSynced] public int hidingTimeSecs = 45;
    [UdonSynced] public int gateCooldownTimeSecs = 15;

    [UdonSynced] public float gameTime = 0f;
    [UdonSynced] public float hidingTime = 0f;
    [UdonSynced] public int nHunters = 1;
    [UdonSynced] public int nFlashlights = 0;
    public HunterItems[] hunterItems;
    public Torch[] flashlights;
    public Torch defaultFlashlight;
    public Transform hunterSpawn;
    public Transform huntedSpawn;
    public Transform gameRoomSpawn;
    public DataList players = new DataList(){};
    public DataList hunters = new DataList(){};
    [UdonSynced] public string players_json = "";
    [UdonSynced] public string hunters_json = "";

    public float hunterHeight = 3.0f;
    public float innocentHeight = 0.5f;
    public LocalPlayerType localPlayerType = LocalPlayerType.NON_PARTICIPANT;

    public float playerStartHeight = 1.84f;
    public GameObject hunterKeepawayZone;
    public PlayerStats playerStats;

    public override void OnPreSerialization()
    {
        if (VRCJson.TrySerializeToJson(players, JsonExportType.Minify, out DataToken result))
        {
            players_json = result.String;
        }
        else
        {
            Debug.LogError(result.ToString());
        }
        if (VRCJson.TrySerializeToJson(hunters, JsonExportType.Minify, out DataToken result2))
        {
            hunters_json = result2.String;
        }
        else
        {
            Debug.LogError(result2.ToString());
        }
    }

    public override void OnDeserialization()
    {
        if(VRCJson.TryDeserializeFromJson(players_json, out DataToken result))
        {
            players = result.DataList;
            for (int i=0; i < players.Count; i++) {
                players[i] = (int) players[i].Double;
            }
        }
        else
        {
            Debug.LogError(result.ToString());
        }
        if(VRCJson.TryDeserializeFromJson(hunters_json, out DataToken result2))
        {
            hunters = result2.DataList;
            for (int i=0; i < hunters.Count; i++) {
                hunters[i] = (int) hunters[i].Double;
            }
        }
        else
        {
            Debug.LogError(result2.ToString());
        }
        Debug.LogFormat("{0}: OnDeserialization", name);
    }

    public override void OnAvatarChanged(VRCPlayerApi player)
    {
        if (gameInProgress && player.isLocal && players.Contains(player.playerId)) {
            
            if (hunters.Contains(player.playerId)) {
                Networking.LocalPlayer.SetAvatarEyeHeightByMeters(hunterHeight);
            }
            else {
                Networking.LocalPlayer.SetAvatarEyeHeightByMeters(innocentHeight);
            }
        }
    }

    public override void OnPlayerTriggerExit(VRCPlayerApi player)
    {
        if (!gameInProgress) {
            if (player.isLocal) {
                localPlayerInZone = false;
            }
            players.Remove(player.playerId);
            UpdateText();
        }
    }

    public override void OnPlayerTriggerStay(VRCPlayerApi player)
    {
        if (!gameInProgress) {
            if (player.isLocal) {
                localPlayerInZone = true;
            }
            if (!players.Contains(player.playerId)) {
                players.Add(player.playerId);
            }
            UpdateText();
        }
    }

    public override void OnPlayerLeft(VRCPlayerApi player)
    {
        if (!gameInProgress) {
            if (player != null && players.Contains(player.playerId)) {
                players.Remove(player.playerId);
                UpdateText();
            }
        }
    }

    public void ClearPlayers() {
        players.Clear();
        gateManager.containedPlayers.Clear();
    }

    public void UpdateText() {
        text.text = string.Format("Players\n{0}", players.Count);
    }

    public void StartGame() {
        Debug.LogFormat("{0}: StartGame", name);
        if (Networking.LocalPlayer.isMaster && players.Count == 0) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EndGame");
            
            Debug.LogFormat("{0}: Not enough players to start", name);
            return;
        }
        gameTime = gameLengthMin * 60f;
        hidingTime = hidingTimeSecs;
        hidingPhase = true;
        ambience.rampTime = hidingTime;
        ambience.Play();
        if (Networking.LocalPlayer.isMaster) {
            MasterStart();
        }
        victorySplash.text = "";
        bubblePool.ActiveEgg();
        SpawnPlayers();
    }

    public void MasterStart() {
        Debug.LogFormat("{0}: MasterStart", name);
        RandomizeHunters();
        nFlashlights = players.Count - nHunters;
        SpawnFlashlights();
        RequestSerialization();
        
        gateManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ShutAllGates");
    }

    public void RandomizeHunters() {
        Debug.LogFormat("{0}: RandomizeHunters", name);
        hunters.Clear();
        int iterLimit = 100;
        for (int h=0;  h < nHunters; h++) {
            int randPlayer = players[Random.Range(0, players.Count)].Int;
            int iter = 0;
            while (hunters.Contains(randPlayer))
            {
                randPlayer = players[Random.Range(0, players.Count)].Int;
                iter++;
                // No infinite loops in my house.
                if (iter > iterLimit) {
                    break;
                }
            }
            hunters.Add(randPlayer);
            hunterItems[h].AssignHunter(VRCPlayerApi.GetPlayerById(randPlayer));
        }
    }

    public void SpawnPlayers() {
        Debug.LogFormat("{0}: SpawnPlayers", name);
        // if (!gameInProgress) {
        //     Debug.LogFormat("{0}: Tried to spawn while game not in progress, initializing backup start", name);
        //     StartGame();
        // }
        int localPlayerId = Networking.LocalPlayer.playerId;
        playerStartHeight = Networking.LocalPlayer.GetAvatarEyeHeightAsMeters();
        if (players.Contains(localPlayerId)) {
            if (hunters.Contains(localPlayerId)) {
                localPlayerType = LocalPlayerType.HUNTER;
                Networking.LocalPlayer.SetManualAvatarScalingAllowed(false);
                Networking.LocalPlayer.SetAvatarEyeHeightByMeters(hunterHeight);
                Networking.LocalPlayer.TeleportTo(hunterSpawn.position, hunterSpawn.rotation);
                hunterKeepawayZone.SetActive(true);
                playerStats.SetBigSpeed();
            }
            else {
                localPlayerType = LocalPlayerType.INNOCENT_FREE;
                Networking.LocalPlayer.SetManualAvatarScalingAllowed(false);
                Networking.LocalPlayer.SetAvatarEyeHeightByMeters(innocentHeight);
                Networking.LocalPlayer.TeleportTo(huntedSpawn.position, huntedSpawn.rotation);
                hunterKeepawayZone.SetActive(false);
                playerStats.SetSmallSpeed();
            }
        }
        else {  
            localPlayerType = LocalPlayerType.NON_PARTICIPANT;
            Networking.LocalPlayer.SetManualAvatarScalingAllowed(true);
            playerStats.SetDefaultSpeed();
        }
        Debug.LogFormat("{0}: Spawned Player. gameInProgress: {1} | localPlayerType: {2}", name, gameInProgress, localPlayerType.ToString());
    }

    public void ResetFlashlights() {
        Debug.LogFormat("{0}: ResetFlashlights", name);
        foreach (Torch flashlight in flashlights) {
            flashlight.Disable();
            Networking.SetOwner(Networking.GetOwner(gameObject), flashlight.gameObject);
        }
    }

    [ContextMenu("Spawn Flashlights")]
    public void SpawnFlashlights() {
        Debug.LogFormat("{0}: SpawnFlashlights", name);
        ResetFlashlights();
        if (nFlashlights > 0) {
            defaultFlashlight.Enable();
            int nFlashlightsEnabled = 1;
            int iter = 0;
            int maxIter = 300;
            int randomFlashlightIndex = 0;
            while (nFlashlightsEnabled < nFlashlights) {
                randomFlashlightIndex = Random.Range(0, flashlights.Length);
                if (!flashlights[randomFlashlightIndex].isEnabled) {
                    flashlights[randomFlashlightIndex].Enable();
                    nFlashlightsEnabled += 1;
                }
                if (iter > maxIter) {
                    break;
                }
                iter++;
            }
        }
    }

    public void EndHidingTime() {
        Debug.LogFormat("{0}: EndHidingTime", name);
        hidingPhase = false;
        workLights.LightsOff();
        
        if (Networking.LocalPlayer.isMaster) {
            EndHidingTimeMaster();
        }
        
        bubblePool.ActivateBubbles();
    }

    public void EndHidingTimeMaster() {
        Debug.LogFormat("{0}: EndHidingTimeMaster", name);
        foreach (HunterItems items in hunterItems) {
            if (items.hunterAssigned) {
                items.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "ActivateItems");
            }
        }
        // bubblePool.ActivateBubbles();
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SpawnHunters");
    }

    public void DelayHunterSpawn() {
        Networking.LocalPlayer.TeleportTo(huntedSpawn.position, huntedSpawn.rotation);
    }

    public void SpawnHunters() {
        Debug.LogFormat("{0}: SpawnHunters", name);
        if (hunters.Contains(Networking.LocalPlayer.playerId)) {
            SendCustomEventDelayedSeconds("DelayHunterSpawn", 2f);
        }
    }

    public void SendPlayerHome() {
        Debug.LogFormat("{0}: Returning player to spawn hub", name);
        if (localPlayerType != LocalPlayerType.NON_PARTICIPANT) {
            Networking.LocalPlayer.TeleportTo(gameRoomSpawn.transform.position, gameRoomSpawn.transform.rotation);
        }
    }

    public void EndGame() {
        
        Debug.LogFormat("{0}: EndGame", name);
        gameInProgress = false;
        hidingPhase = false;
        ambience.Stop();
        workLights.LightsOn();
        Networking.LocalPlayer.SetManualAvatarScalingAllowed(true);
        Networking.LocalPlayer.SetAvatarEyeHeightByMeters(playerStartHeight);
        playerStats.SetDefaultSpeed();
        
        if (Networking.LocalPlayer.isMaster) {
            EndGameMaster();
        }
        ClearPlayers();
        SendCustomEventDelayedSeconds("ClearPlayers", 3f);
        ResetFlashlights();
        RequestSerialization();
    }

    public void EndGameMaster() {
        
        Debug.LogFormat("{0}: EndGameMaster", name);
        foreach (HunterItems items in hunterItems) {
            items.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "DeactivateItems");
        }
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "SendPlayerHome");
        bubblePool.DeactivateBubbles();
        bubblePool.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InactiveEgg");
    }

    public override void OnPlayerRespawn(VRCPlayerApi player)
    {
        if (gameInProgress && player.isLocal) {
            player.TeleportTo(playerTracker.transform.position, playerTracker.transform.rotation);
            Debug.LogFormat("{0}: Respawn prevented!", name);
        }
    }

    public void InnocentsWin() {
        victorySplash.color = Color.cyan;
        victorySplash.text = "Innocents\nWin!";
        innocentWinSound.Play();
    }

    public void HuntersWin() {
        victorySplash.color = Color.red;
        victorySplash.text = "Hunters\nWin!";
        hunterWinSound.Play();
    }

    public void Update() {
        if (gameInProgress && !hidingPhase) {
            gameTime -= Time.deltaTime;
            if (gameTime <= 0f) {
                gameTime = 0f;
                if (Networking.LocalPlayer.isMaster) {
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EndGame");
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InnocentsWin");
                }
            }
        }
        else if (gameInProgress && hidingPhase) {
            hidingTime -= Time.deltaTime;
            if (hidingTime <= 0f) {
                hidingTime = 0f;
                if (Networking.LocalPlayer.isMaster) {
                    SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EndHidingTime");
                }
            }
        }
    }

    public void VictoryCondition(bool instantWin) {
        if (instantWin) {
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EndGame");
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "HuntersWin");
        }
        else {
            // Check if all the hunters left
            int nHunters = 0;
            for (int i=0; i < hunters.Count; i++) {
                if (VRCPlayerApi.GetPlayerById(hunters[i].Int) != null) {
                    nHunters += 1;
                }
            }
            if (nHunters == 0) {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "EndGame");
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "InnocentsWin");
            }
        }
    }

}
