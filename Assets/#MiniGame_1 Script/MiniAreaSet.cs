using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAreaSet : MonoBehaviour
{
    float destroyDistance = 50f;

    public MiniSetSpawn setSpawn;
    public Transform playerPos;

    public void Setup(MiniSetSpawn setSpawn, Transform playerPos)
    {
        this.setSpawn = setSpawn;
        this.playerPos = playerPos;
    }

    void Update()
    {
        if (playerPos.position.z - transform.position.z >= destroyDistance)
        {
            setSpawn.SpawnArea();

            Destroy(gameObject);
        }
    }
}
