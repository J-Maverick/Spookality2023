
using UdonSharp;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class StartButton : UdonSharpBehaviour
{

    public float startTime = 5f;
    public float startTimer = 5f;
    public bool starting = false;
    [UdonSynced] public bool masterOnly = true;
    public GameManager gameManager;
    public Text timerText;

    public override void Interact()
    {
        if ((masterOnly && Networking.LocalPlayer.isMaster) || !masterOnly) {
            TryStart();
        }
    }

    public void TryStart() {
        if (!gameManager.gameInProgress) {
            if (!starting) {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Starting");
            }
            else {
                SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "Stop");
            }
        }
    }

    public void Starting() {
        starting = true;
        startTimer = startTime;
    }

    public void Stop() {
        starting = false;
        startTimer = startTime;
    }

    public void Update() {
        if (starting) {
            startTimer -= Time.deltaTime;
            if (startTimer <= 0f) {
                starting = false;
                if (Networking.GetOwner(gameManager.gameObject).isLocal && gameManager.players.Count > gameManager.nHunters) {
                    gameManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StartGame");
                }
            }
            timerText.text = string.Format("Starting in... {0}", (int) startTimer);
        }
        else if (gameManager.gameInProgress) {
            timerText.text = "Game in progress...";
        }
        else {
            timerText.text = "";
        }
    }

}
