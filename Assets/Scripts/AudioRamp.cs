
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AudioRamp : UdonSharpBehaviour
{
    public AudioSource audioSource;
    public float targetVolume = 1f;
    public float timeRamped = 0f;
    public float rampTime = 30f;
    public bool ramping = false;
    public bool playOnAwake = false;

    void Start() {
        SendCustomEventDelayedSeconds("Awake", 5f);
    }

    public void Awake() {
        Debug.LogFormat("{0}: Awake!", name);
        if (playOnAwake) {
            Play();
        }
    }


    public void Play() {
        audioSource.volume = 0f;
        ramping = true;
        timeRamped = 0f;
        audioSource.Play();
    }

    public void Stop() {
        audioSource.Stop();
    }

    void Update() {
        if (ramping) {
            audioSource.volume = Mathf.Clamp(Mathf.Lerp(0f, targetVolume, timeRamped / rampTime), 0f, 1f);
            
            timeRamped += Time.deltaTime;
            if (audioSource.volume == targetVolume) {
                ramping = false;
            }
        }
    }
}
