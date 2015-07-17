using UnityEngine;
using System.Collections;
using PlayerScripts;

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateTargetAndFOV();
    }

    /// <summary>
    /// Average the player positions and focus on that point
    /// TODO: calculate FOV based on how far apart players are
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
        target /= playerStatuses.Length;
        transform.LookAt(target);

        float mod = maxX - minX + maxZ - minZ;
        Camera.main.fieldOfView= 20f + (mod * 1.5f);
    }
}