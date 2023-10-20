
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AudioIsPlayingTest : UdonSharpBehaviour
{
    public AudioSource audioSource;

    public override void Interact()
    {
        if (!audioSource.isPlaying) {
            audioSource.Play();
        }
    }
}
