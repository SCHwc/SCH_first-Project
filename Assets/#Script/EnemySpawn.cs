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

        // 체력바가 쫓아다닐 대상을 소환된 enemy로 지정
        hpBarClone.GetComponent<EnemyHpBar>().Setup(enemy.transform);
        // 소환된 enemy의 체력정보를 표시하도록 설정
        hpBarClone.GetComponent<EnemyHpBarView>().Setup(enemy.GetComponent<Enemy>());
    }
}
