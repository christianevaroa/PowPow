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

    int lastHealth;

    Animator anim;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        Debug.Log(transform.position);
	}
	
	// Update is called once per frame
	void Update () {
        if (status != null && lastHealth != status.health)
        {
            Debug.Log(transform.position);
            playerHPText.text = status.health.ToString();
            lastHealth = status.health;
            anim.SetTrigger("Shake");
        }
	}

    public void SetStatus(PlayerStatus ps)
    {
        status = ps;
        lastHealth = status.health;
        Debug.Log(transform.position);
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
