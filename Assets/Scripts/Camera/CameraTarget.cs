using UnityEngine;
using System.Collections;
using PlayerScripts;
using System;

public class CameraTarget : MonoBehaviour
{

    public float smallestFOV;
    public float largestFOV;
    public PlayerStatus[] playerStatuses;
    Vector3 target;

    // Use this for initialization
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerStatuses = new PlayerStatus[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            playerStatuses[i] = players[i].GetComponent<PlayerStatus>();
            Debug.Log(i + ", " + playerStatuses[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (RoundManager.playersStillAlive > 1)
        {
            CalculateTargetAndFOV();
        }
        else
        {
            StartCoroutine(MoveToWinner());
        }
    }

    /// <summary>
    /// Average the player positions and focus on that point
    /// </summary>
    void CalculateTargetAndFOV()
    {
        target.Set(0, 0, 0);
        float minX = float.PositiveInfinity;
        float minZ = float.PositiveInfinity;
        float maxX = float.NegativeInfinity;
        float maxZ = float.NegativeInfinity;

        // Calculate average
        foreach (PlayerStatus p in playerStatuses)
        {
            if (p.health > 0) 
            {
                target += p.transform.position;
                minX = Mathf.Min(p.transform.position.x, minX);
                minZ = Mathf.Min(p.transform.position.z, minZ);
                maxX = Mathf.Max(p.transform.position.x, maxX);
                maxZ = Mathf.Max(p.transform.position.z, maxZ);
            } 
        }

        //TODO: probably not the best way
        target /= RoundManager.playersStillAlive > 0 ? RoundManager.playersStillAlive : 1;
        transform.LookAt(target);

        float mod = maxX - minX + maxZ - minZ;
        Camera.main.fieldOfView= 20f + (mod * 1.5f);
    }

    IEnumerator MoveToWinner()
    {
        GameObject winner = FindWinner();
        // TODO: make sure this condition is always true...
        if (winner != null)
        {
            Vector3 winnerPos = winner.transform.position + Vector3.up;
            Vector3 pos = winnerPos + (winner.transform.forward * 2);
            Vector3 initialPos = transform.position;
            float pct = 0f;
            float initialFov = Camera.main.fieldOfView;

            // Move camera to focus on winner over 2 seconds
            while (transform.position != pos)
            {
                transform.position = Vector3.Lerp(initialPos, pos, pct);
                transform.LookAt(winnerPos);
                Camera.main.fieldOfView = Mathf.Lerp(initialFov, 60, pct);
                pct += Time.deltaTime / 2;
                yield return null;
            }
        }
    }

    private GameObject FindWinner()
    {
        foreach (PlayerStatus p in playerStatuses)
        {
            if (p.health > 0)
                return p.gameObject;
        }
        return null;
    }
}