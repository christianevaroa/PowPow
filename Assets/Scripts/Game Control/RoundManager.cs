﻿using UnityEngine;
using UnityEngine.UI;
using PlayerScripts;
using System.Collections;
using System.Collections.Generic;
using System;

public class RoundManager : MonoBehaviour {

    public Canvas HUD;
    public Text getReadyText;
    public Text timeLeftText;
    public Animator getReadyTextAnimator;
    public int countDownSeconds;
    public float timer;

    public GameObject playerPanelPrefab;

    RoundAudioManager ram;
    RoundMusicManager rmm;

    public static int playersStillAlive;
    public PlayerStatus[] playerStatuses;
    [HideInInspector]
    public UIPlayerInfo[] playerPanels;

    bool roundRunning;
    public float yOffsetPercent;

    void Awake()
    {
        ram = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<RoundAudioManager>();
        rmm = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<RoundMusicManager>();
        FindPlayers();
    }

	// Use this for initialization
	void Start () {
        PopulateHUD();
        TimerToString();
        StartCoroutine(StartCountDown());
	}
	
	// Update is called once per frame
	void Update () {
        if (roundRunning)
        {
            // Nested if because I might check other stuff too...
            if (playersStillAlive == 1)
            {
                EndRound();
            }
        }
	}

    

    /// <summary>
    /// Find all the gameobjects in the scene tagged player and put their playerstatus scripts into the array
    /// </summary>
    void FindPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerStatuses = new PlayerStatus[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            PlayerStatus current = players[i].GetComponent<PlayerStatus>();
            //HACK: would be nicer to make them comparable and sort the array but ehh this works...
            playerStatuses[current.playerNumber - 1] = current;

            playersStillAlive++;
        }
    }

    /// <summary>
    /// Instantiate an HP/MP UI panel for each player
    /// </summary>
    void PopulateHUD()
    {
        playerPanels = new UIPlayerInfo[playerStatuses.Length];

        float xSpacing = Screen.width / (playerStatuses.Length + 1);
        float ySpacing = Screen.height * yOffsetPercent;

        for (int i = 0; i < playerStatuses.Length; i++)
        {
            Vector3 pos = new Vector3((i+1) * xSpacing, ySpacing, 0f);
            Debug.Log(i + ": " + pos);
            GameObject panel = Instantiate(playerPanelPrefab, pos, Quaternion.identity) as GameObject;
            panel.transform.SetParent(HUD.transform);
            UIPlayerInfo panelInfo = panel.GetComponent<UIPlayerInfo>();
            playerPanels[i] = panelInfo;
            panelInfo.playerNumText.text = " P"+playerStatuses[i].playerNumber;
            panelInfo.playerHPText.text = playerStatuses[i].health.ToString();
            panelInfo.SetStatus(playerStatuses[i]);
        }
    }

    void StartRound()
    {
        roundRunning = true;
        getReadyText.text = "GO!!";
        foreach (PlayerStatus p in playerStatuses)
        {
            p.StartRound();
        }
        getReadyTextAnimator.SetTrigger("Exit");
        StartCoroutine(RoundTimer());
    }

    void EndRound()
    {
        foreach (PlayerStatus p in playerStatuses)
        {
            p.DisableMovement();
        }
        for (int i = 0; i < playerStatuses.Length; i++)
        {
            if (playerStatuses[i].health > 0) 
            {
                playerPanels[i].Win(true);
                playerStatuses[i].Dance();
            }
            else
            {
                playerPanels[i].Win(false);
            }
        }
        rmm.PlayOutro();
        roundRunning = false;
    }

    void TimerToString()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        timeLeftText.text = string.Format("{0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
    }

    IEnumerator StartCountDown()
    {
        int counter = countDownSeconds;
        while (counter > 0)
        {
            getReadyText.text = "GET READY!!\n" + counter;
            ram.PlayOneShot(ram.TICK_LOW);
            yield return new WaitForSeconds(1f);
            counter--;
        }
        ram.PlayOneShot(ram.TICK_HIGH);
        StartRound();
    }

    IEnumerator RoundTimer() 
    {
        while (roundRunning && timer > 0f)
        {
            TimerToString();
            timer -= Time.deltaTime;
            yield return null;
        }
        if (roundRunning)
        {
            EndRound();
        }
    }

    public static void Death()
    {
        playersStillAlive--;
    }
}
