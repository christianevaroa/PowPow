using UnityEngine;
using UnityEngine.UI;
using PlayerScripts;
using System.Collections;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour {

    public Canvas HUD;
    public Text getReadyText;
    public Animator getReadyTextAnimator;
    public int countDownSeconds;

    public GameObject playerPanelPrefab;

    RoundAudioManager ram;

    public PlayerStatus[] playerStatuses;
    public UIPlayerInfo[] playerPanels;

    void Awake()
    {
        ram = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<RoundAudioManager>();
        FindPlayers();
    }

	// Use this for initialization
	void Start () {
        PopulateHUD();
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
            //HACK would be nicer to make them comparable and sort the array but ehh this works...
            playerStatuses[current.playerNumber - 1] = current;
        }
    }

    /// <summary>
    /// Instantiate an HP/MP UI panel for each player
    /// </summary>
    void PopulateHUD()
    {
        playerPanels = new UIPlayerInfo[playerStatuses.Length];

        float xSpacing = Screen.width / (playerStatuses.Length + 1);
        float ySpacing = Screen.height * 0.1f;

        for (int i = 0; i < playerStatuses.Length; i++)
        {
            Vector3 pos = new Vector3((i+1) * xSpacing, ySpacing, 0f);
            GameObject panel = Instantiate(playerPanelPrefab, pos, Quaternion.identity) as GameObject;
            panel.transform.SetParent(HUD.transform);
            UIPlayerInfo panelInfo = panel.GetComponent<UIPlayerInfo>();
            panelInfo.playerNumText.text = " P"+playerStatuses[i].playerNumber;
            panelInfo.playerHPText.text = playerStatuses[i].health.ToString();
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
