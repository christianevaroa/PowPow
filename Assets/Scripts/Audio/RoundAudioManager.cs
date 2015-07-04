using UnityEngine;
using System.Collections;

public class RoundAudioManager : MonoBehaviour {

    public AudioClip TICK_LOW;
    public AudioClip TICK_HIGH;

    private AudioSource source;

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayOneShot(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
