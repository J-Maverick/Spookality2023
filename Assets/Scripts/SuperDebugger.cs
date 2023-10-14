
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;
using VRC.SDK3.Data;
using VRC.SDK3.Components;

public class SuperDebugger : UdonSharpBehaviour
{

    public Text debugText;
    public PlayerKillBubble playerKillBubble;
    public GameManager gameManager;
    public GateManager gateManager;
    public int frameOffset = 0;

    // Values to share from owning player
    // Killbubble things
    [UdonSynced] public string _usingPlayer = "";
    [UdonSynced] public bool bubbleActive = false;
    [UdonSynced] bool bubbleColliderActive = false;


    // gameManager things
    [UdonSynced] public bool gameInProgress = false;
    [UdonSynced] public bool hidingPhase = false;
    [UdonSynced] public int gameLengthMin = 5;
    [UdonSynced] public int hidingTimeSecs = 45;
    [UdonSynced] public int gateCooldownTimeSecs = 15;

    [UdonSynced] public float gameTime = 0f;
    [UdonSynced] public float hidingTime = 0f;
    [UdonSynced] public int nHunters = 1;
    [UdonSynced] public int nFlashlights = 0;
    [UdonSynced] public string players_json = "";
    [UdonSynced] public string hunters_json = "";
    [UdonSynced] string localPlayerType = "";

    [UdonSynced] public float playerStartHeight = 1.84f;

    // gateManager things
    [UdonSynced] public string containedPlayers_json = "";
    [UdonSynced] public float timer = 0f;

    void Start() {
        frameOffset = Random.Range(-10, 10);
    }

    public override void OnPreSerialization() {
        if (VRCJson.TrySerializeToJson(gameManager.players, JsonExportType.Minify, out DataToken result))
        {
            players_json = result.String;
        }
        else
        {
            Debug.LogError(result.ToString());
        }
        if (VRCJson.TrySerializeToJson(gameManager.hunters, JsonExportType.Minify, out DataToken result2))
        {
            hunters_json = result2.String;
        }
        else
        {
            Debug.LogError(result2.ToString());
        }
        if (VRCJson.TrySerializeToJson(gateManager.containedPlayers, JsonExportType.Minify, out DataToken result3))
        {
            containedPlayers_json = result3.String;
        }
        else
        {
            Debug.LogError(result3.ToString());
        }

        if (playerKillBubble._usingPlayer != null) {
            _usingPlayer = string.Format("{0}[{1}]", playerKillBubble._usingPlayer.displayName, playerKillBubble._usingPlayer.playerId);
        }
        else {
            _usingPlayer = "null";
        }
        bubbleActive = playerKillBubble.bubbleActive;
        bubbleColliderActive = playerKillBubble.bubbleCollider.enabled;


        // gameManager things
        gameInProgress = gameManager.gameInProgress;
        hidingPhase = gameManager.hidingPhase;
        gameLengthMin = gameManager.gameLengthMin;
        hidingTimeSecs = gameManager.hidingTimeSecs;
        gateCooldownTimeSecs = gameManager.gateCooldownTimeSecs;
        gameTime = gameManager.gameTime;
        hidingTime = gameManager.hidingTime;
        nHunters = gameManager.nHunters;
        nFlashlights = gameManager.nFlashlights;
        players_json = gameManager.players_json;
        hunters_json = gameManager.hunters_json;
        localPlayerType = gameManager.localPlayerType.ToString();

        playerStartHeight = gameManager.playerStartHeight;

        // gateManager things
        timer = gateManager.timer;
    }


    void UpdateDebugText()
    {
        if (Networking.GetOwner(gameObject) != playerKillBubble._usingPlayer) {
            Networking.SetOwner(playerKillBubble._usingPlayer, gameObject);
        }
        string text = "";

        text = UpdatePlayerBubbleData(text);
        text = UpdateGameManagerData(text);
        text = UpdateGateManagerData(text);
        text = UpdateWithLocalValues(text);
        debugText.text = text;
    }

    string UpdatePlayerBubbleData(string text) {
        text += string.Format(
@"Bubble: {0}
 _usingPlayer: {1}[{2}]
 bubbleActive: {3}
 bubbleColliderActive: {4}
", 
            playerKillBubble.name, 
            playerKillBubble._usingPlayer.displayName, 
            playerKillBubble._usingPlayer.playerId, 
            playerKillBubble.bubbleActive, 
            playerKillBubble.bubbleCollider.enabled);
        return text;
    }
    string UpdateGameManagerData(string text) {
        text += string.Format(
@"GameManager:
 gameInProgress: {0}
 hidingPhase: {1}
 gameLengthMin: {2}
 hidingTimeSecs: {3}
 gateCooldownTimeSecs: {4}
 gameTime: {5}
 hidingTime: {6}
 nHunters: {7}
 nFlashlights: {8}
 players_json: {9}
 hunters_json: {10}
 localPlayerType: {11}
 playerStartHeight: {12}
",             
            gameManager.gameInProgress,
            gameManager.hidingPhase,
            gameManager.gameLengthMin,
            gameManager.hidingTimeSecs,
            gameManager.gateCooldownTimeSecs,
            gameManager.gameTime,
            gameManager.hidingTime,
            gameManager.nHunters,
            gameManager.nFlashlights,
            gameManager.players_json,
            gameManager.hunters_json,
            gameManager.localPlayerType.ToString(),
            gameManager.playerStartHeight
            );
        return text;
    }
    string UpdateGateManagerData(string text) {
        string gateManagerContained = "";
        if (VRCJson.TrySerializeToJson(gateManager.containedPlayers, JsonExportType.Minify, out DataToken result3))
        {
            gateManagerContained = result3.String;
        }
        else
        {
            Debug.LogError(result3.ToString());
        }
        text += string.Format(
@"GateManager:
 containedPlayers: {0}
 timer: {1}
", 
            gateManagerContained, 
            gateManager.timer);
        return text;
    }


    string UpdateWithLocalValues(string text) {
        text += "\n\nOwner Data: ";
        text = UpdateLocalPlayerBubbleData(text);
        text = UpdateLocalGameManagerData(text);
        text = UpdateLocalGateManagerData(text);
        return text;
    }
    string UpdateLocalPlayerBubbleData(string text) {
        text += string.Format(
@"Bubble: {0}
 _usingPlayer: {1}
 bubbleActive: {2}
 bubbleColliderActive: {3}
", 
            name, 
            _usingPlayer,
            bubbleActive, 
            bubbleColliderActive);
        return text;
    }
    string UpdateLocalGameManagerData(string text) {
        text += string.Format(
@"GameManager:
 gameInProgress: {0}
 hidingPhase: {1}
 gameLengthMin: {2}
 hidingTimeSecs: {3}
 gateCooldownTimeSecs: {4}
 gameTime: {5}
 hidingTime: {6}
 nHunters: {7}
 nFlashlights: {8}
 players_json: {9}
 hunters_json: {10}
 localPlayerType: {11}
 playerStartHeight: {12}
",             
            gameInProgress,
            hidingPhase,
            gameLengthMin,
            hidingTimeSecs,
            gateCooldownTimeSecs,
            gameTime,
            hidingTime,
            nHunters,
            nFlashlights,
            players_json,
            hunters_json,
            localPlayerType.ToString(),
            playerStartHeight
            );
        return text;
    }
    string UpdateLocalGateManagerData(string text) {
        text += string.Format(
@"GateManager:
 containedPlayers: {0}
 timer: {1}
", 
            containedPlayers_json, 
            timer);
        return text;
    }

    public void Update() {
        if (Time.frameCount % (50 + frameOffset) == 0) {
            UpdateDebugText();
        }
        if (Networking.LocalPlayer != null) {
            transform.rotation = Networking.LocalPlayer.GetRotation();
            gameManager.bubblePool.InactiveEgg();
        }
    }
}
