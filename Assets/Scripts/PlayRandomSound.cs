
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class PlayRandomSound : UdonSharpBehaviour
{
    public AudioClip[] clips = null;
    public AudioSource audioSource = null;

    public void Play() {
        if (!audioSource.isPlaying) {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();
        }
    }
}
