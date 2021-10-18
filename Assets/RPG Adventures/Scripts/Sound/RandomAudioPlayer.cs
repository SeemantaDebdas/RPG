using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [System.Serializable]
    public class SoundBank
    {
        public string name;
        public AudioClip[] audioClips;
    }

    public bool isPlaying;
    public bool canPlay;
    public SoundBank soundBank = new SoundBank();
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayRandomClip()
    {
        var clip = soundBank.audioClips[Random.Range(0, soundBank.audioClips.Length)];
        audioSource.clip = clip;
        audioSource.Play();
    }
}
