
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class Torch : UdonSharpBehaviour
{
    public GameObject lightObject;
    public AudioSource lightClickSound;
    public Material material;
    public MeshRenderer meshRenderer;
    public Rigidbody rb;
    public Collider meshCollider;

    public Vector3 spawnLocation;
    public Quaternion spawnRotation;
    public VRCPickup pickup;

    [UdonSynced, FieldChangeCallback(nameof(lightOn))]
    private bool _lightOn = false;
    public GameManager gameManager;

    public bool lightOn
    {
        set
        {
            _lightOn = value;
            if (_lightOn) {
                LightOn();
            }
            else {
                LightOff();
            }
        }
        get => _lightOn;
    }

    [UdonSynced, FieldChangeCallback(nameof(isEnabled))]
    private bool _isEnabled = false;

    public bool isEnabled
    {
        set
        {
            _isEnabled = value;
            if (_isEnabled) {
                Enable();
            }
            else {
                Disable();
            }
        }
        get => _isEnabled;
    }


    public void Start() {
        material = meshRenderer.material;
        spawnLocation = transform.position;
        spawnRotation = transform.rotation;
        if (Networking.LocalPlayer.IsUserInVR()) {
            pickup.orientation = VRC_Pickup.PickupOrientation.Any;
        }
        if (isEnabled) {
            Enable();
        }
        else {
            Disable();
        }
    }

    [ContextMenu("Toggle Flashlight")]
    public override void OnPickupUseDown()
    {
        lightOn = !lightOn;
    }

    public void Reset() {
        material.DisableKeyword("_EMISSION");
        lightObject.SetActive(false);
        transform.SetPositionAndRotation(spawnLocation, spawnRotation);
    }

    public void LightOn() {
        material.EnableKeyword("_EMISSION");
        lightObject.SetActive(true);
        lightClickSound.Play();
    }

    public void LightOff() {
        material.DisableKeyword("_EMISSION");
        lightObject.SetActive(false);
        lightClickSound.Play();
    }

    public void Enable() {
        meshRenderer.enabled = true;
        rb.isKinematic = false;
        meshCollider.enabled = true;
        isEnabled = true;
        pickup.pickupable = true;
        SendCustomEventDelayedSeconds("DisableForHunters", 5f);
    }

    public void Disable() {
        meshRenderer.enabled = false;
        rb.isKinematic = true;
        meshCollider.enabled = false;
        isEnabled = false;
        pickup.Drop();
        Reset();
    }

    public void DisableForHunters() {
        if (gameManager.localPlayerType == LocalPlayerType.HUNTER) {
            pickup.pickupable = false;
        }
    }
}
