using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;
    public GameObject hpBar;
    public Transform canvas;
    public float spawnTime = 5f;

    BossZonePortal bossBattle;

    void Awake()
    {
        StartCoroutine(SpawnEnemy());
    }

    void Update()
    {

    }

    IEnumerator SpawnEnemy()
    {
        while (true)
        {
            if (!bossBattle.isFight)
            {
                GameObject enemyClone = Instantiate(enemy, transform);
                EnemyHp(enemyClone);
                yield return new WaitForSeconds(spawnTime);
            }
        }
    }

    void EnemyHp(GameObject enemy)
    {
        GameObject hpBarClone = Instantiate(hpBar);

        hpBarClone.transform.SetParent(canvas);
        hpBarClone.transform.localScale = Vector3.one;

        // ü�¹ٰ� �Ѿƴٴ� ����� ��ȯ�� enemy�� ����
        hpBarClone.GetComponent<EnemyHpBar>().Setup(enemy.transform);
        // ��ȯ�� enemy�� ü�������� ǥ���ϵ��� ����
        hpBarClone.GetComponent<EnemyHpBarView>().Setup(enemy.GetComponent<Enemy>());
    }
}
