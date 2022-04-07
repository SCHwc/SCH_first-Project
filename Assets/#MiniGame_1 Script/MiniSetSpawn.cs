using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniSetSpawn : MonoBehaviour
{
    public GameObject[] areaSet; // ���� ������ �迭
    int spawnCount = 3; // ���� ����
    float zLength = 50f; // �����Ǵ� ���� ������ �Ÿ�
    int setIndex = 1; // ���� �ε��� (��ġ�Ǵ� ������ z ��ġ ���꿡 ���)

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
