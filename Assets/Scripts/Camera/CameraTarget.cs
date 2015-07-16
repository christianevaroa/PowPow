using UnityEngine;
using System.Collections;

public class CameraTarget : MonoBehaviour
{

    public float smallestFOV;
    public float largestFOV;
    public Transform[] playerTransforms;
    Vector3 target;

    // Use this for initialization
    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerTransforms = new Transform[players.Length];
        for (int i = 0; i < players.Length; i++)
        {
            playerTransforms[i] = players[i].transform;
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
        foreach (Transform t in playerTransforms)
        {
            target += t.position;
            minX = Mathf.Min(t.position.x, minX);
            minZ = Mathf.Min(t.position.z, minZ);
            maxX = Mathf.Max(t.position.x, maxX);
            maxZ = Mathf.Max(t.position.z, maxZ);
        }
        target /= playerTransforms.Length;
        transform.LookAt(target);

        float mod = maxX - minX + maxZ - minZ;
        Camera.main.fieldOfView= 20f + (mod * 1.5f);
    }
}