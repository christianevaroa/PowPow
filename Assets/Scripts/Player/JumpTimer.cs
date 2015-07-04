using UnityEngine;
using System.Collections;

public class JumpTimer : MonoBehaviour {

    readonly float jumpTime = Mathf.Sqrt(2);
    public float timer { get; private set; }
    public bool complete { get; private set; }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartTiming()
    {
        StartCoroutine(TimeJump());
    }

    IEnumerator TimeJump()
    {
        complete = false;
        timer = -jumpTime;
        while (timer < 0f)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        complete = true;
    }
}
