using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorwObstacleSpawner : MonoBehaviour
{
    public float timer;
    public float speed;
    public Transform[] spawnPos;
    public GameObject obstacle;
    void Start()
    {
        StartCoroutine(GetSpawn());
    }

    IEnumerator GetSpawn()
    {
        while (true)
        {
            for (int i = 0; i < spawnPos.Length; i++)
            {
                GameObject instObj = Instantiate(obstacle, spawnPos[i].position, spawnPos[i].rotation);
                Rigidbody obstacle_rb = instObj.GetComponent<Rigidbody>();
                obstacle_rb.velocity = spawnPos[i].forward * speed;
            }
            yield return new WaitForSeconds(timer);
        }

    }
}
