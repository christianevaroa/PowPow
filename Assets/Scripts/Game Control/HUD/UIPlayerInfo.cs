using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PlayerScripts;

public class UIPlayerInfo : MonoBehaviour {

    public Text playerNumText;
    public Text playerHPText;
    public Text winLoseText;
    public Slider playerMPSlider;

    // The PlayerStatus script that this panel is observing
    PlayerStatus status;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (status != null)
        {
            playerHPText.text = status.health.ToString();
        }
	}

    public void SetStatus(PlayerStatus ps)
    {
        status = ps;
    }

    public void Win(bool won)
    {
        if (won) 
        {
            winLoseText.text = "WIN!!";
        }
        else
        {
            winLoseText.text = "LOSE";
        }

        StartCoroutine(FlashText());
    }

    IEnumerator FlashText()
    {
        while (true)
        {
            winLoseText.enabled = !winLoseText.enabled;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
