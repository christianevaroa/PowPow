using UnityEngine;
using UnityEngine.UI;
using PlayerScripts;
using System.Collections;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {

    public Text getReadyText;
    public Animator getReadyTextAnimator;
    public int countDownSeconds;

    RoundAudioManager ram;

    List<PlayerStatus> playerStatuses = new List<PlayerStatus>();

	// Use this for initialization
	void Start () {
        ram = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<RoundAudioManager>();
        FindPlayers();
        StartCoroutine(StartCountDown());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator StartCountDown()
    {
        int counter = countDownSeconds;
        while(counter > 0){
            getReadyText.text = "GET READY!!\n" + counter;
            ram.PlayOneShot(ram.TICK_LOW);
            yield return new WaitForSeconds(1f);
            counter--;
        }
        ram.PlayOneShot(ram.TICK_HIGH);
        StartRound();
    }

    void FindPlayers()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player"))
        {
            playerStatuses.Add(go.GetComponent<PlayerStatus>());
            Debug.Log("Found player: " + go);
        }
    }

    void StartRound()
    {
        getReadyText.text = "GO!!";
        foreach (PlayerStatus p in playerStatuses)
        {
            p.StartRound();
        }
        getReadyTextAnimator.SetTrigger("Exit");
    }
}
