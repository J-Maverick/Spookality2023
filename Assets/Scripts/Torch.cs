
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

public class Torch : UdonSharpBehaviour
{
    public GameObject lightObject;
    public AudioSource lightClickSound;
    // public Material material;
    public MeshRenderer meshRenderer;
    public Rigidbody rb;
    public Collider meshCollider;

    public Vector3 spawnLocation;
    public Quaternion spawnRotation;
    public VRCPickup pickup;
    public GameManager gameManager;
    // [ColorUsage(true, true)]
    // public Color onEmission;
    // [ColorUsage(true, true)]
    // public Color offEmission;
    public Animator animator;


    [UdonSynced, FieldChangeCallback(nameof(lightOn))]
    private bool _lightOn;
    public bool lightOn
    {
        set
        {
            _lightOn = value;
            if (value) {
                LightOn();
            }
            else {
                LightOff();
            }
        }
        get => _lightOn;
    }

    [UdonSynced, FieldChangeCallback(nameof(isEnabled))]
    private bool _isEnabled;
    public bool isEnabled
    {
        set
        {
            if (value != _isEnabled) {          
                _isEnabled = value;
                if (value) {
                    Enable();
                }
                else {
                    Disable();
                }
            }
        }
        get => _isEnabled;
    }


    public void Start() {
        // material = meshRenderer.material;
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
        if (Networking.LocalPlayer.isMaster) {
            Reset();
        }
    }

    public override void OnPickupUseDown()
    {
        ToggleFlashlight();
    }

    public void ToggleFlashlight() {
        lightOn = !lightOn;
    }

    public void Reset() {
        // material.SetVector("_EmissionColor", offEmission);
        // meshRenderer.material = material;
        animator.SetBool("LightOn", false);
        lightObject.SetActive(false);
        transform.SetPositionAndRotation(spawnLocation, spawnRotation);
    }

    public void LightOn() {
        Debug.LogFormat("{0}: Light On", name);
        animator.SetBool("LightOn", true);
        // material.SetVector("_EmissionColor", onEmission);
        // meshRenderer.material = material;
        lightObject.SetActive(true);
        lightClickSound.Play();
    }

    public void LightOff() {
        Debug.LogFormat("{0}: Light Off", name);
        animator.SetBool("LightOn", false);
        // material.SetVector("_EmissionColor", offEmission);
        // meshRenderer.material = material;
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
