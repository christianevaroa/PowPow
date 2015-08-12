using UnityEngine;
using System.Collections;

public class RoundMusicManager : MonoBehaviour {

    public AudioClip musicClip;
    public AudioClip outroMusic;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(musicClip);
	}

    public void PlayOutro()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(outroMusic);
    }
}
