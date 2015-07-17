using UnityEngine;
using System.Collections;

public class PlayerAudio : MonoBehaviour {

    public AudioClip throwClip;
    public AudioClip[] hitClips;
    public AudioClip death;
    AudioSource audioSource;

	// Use this for initialization
	void Start () {
        audioSource = GetComponent<AudioSource>();
	}

    public void Throw()
    {
        audioSource.PlayOneShot(throwClip);
    }

    public void Die()
    {
        audioSource.PlayOneShot(death);
    }

    public void GetHit()
    {
        audioSource.PlayOneShot(hitClips[Random.Range(0, hitClips.Length - 1)]);
    }
}
