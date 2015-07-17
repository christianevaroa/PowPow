using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerScripts;

public class UIPlayerInfo : MonoBehaviour {

    public Text playerNumText;
    public Text playerHPText;
    public Slider playerMPSlider;

    // The PlayerStatus script that this panel is observing
    PlayerStatus status;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetStatus()
    {

    }
}
