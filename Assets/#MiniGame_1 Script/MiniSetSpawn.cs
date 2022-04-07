using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSetSpawn : MonoBehaviour
{
    public GameObject[] areaSet; // 구역 프리팹 배열
    int spawnCount = 3; // 구역 갯수
    float zLength = 50f; // 스폰되는 구역 사이의 거리
    int setIndex = 1; // 구역 인덱스 (배치되는 구역의 z 위치 연산에 사용)

    public Transform playerPosition;

    void Awake()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnArea();
        }
    }

    public void SpawnArea()
    {
        GameObject area = null;
        int index = Random.Range(0, areaSet.Length);
        area = Instantiate(areaSet[index]);

        area.transform.position = new Vector3(0, 0, setIndex * zLength);

        area.GetComponent<MiniAreaSet>().Setup(this, playerPosition);

        setIndex++;
    }

    void Update()
    {

    }
}
