
using UdonSharp;
using UnityEngine;
using UnityEngine.PlayerLoop;
using VRC.SDKBase;
using VRC.Udon;

public class StartButton : UdonSharpBehaviour
{

    public float startTime = 5f;
    public float startTimer = 5f;
    [UdonSynced] public bool starting = false;
    [UdonSynced] public bool masterOnly = true;
    public GameManager gameManager;

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
                if (Networking.GetOwner(gameManager.gameObject).isLocal) {
                    gameManager.SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "StartGame");
                }
            }
        }
    }

}
