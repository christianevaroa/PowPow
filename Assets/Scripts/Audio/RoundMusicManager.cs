using UnityEngine;
using System.Collections;

public class RoundMusicManager : MonoBehaviour {

    public AudioClip musicClip;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(musicClip);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
