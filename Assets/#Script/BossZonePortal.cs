using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZonePortal : MonoBehaviour
{
    public enum Type { FieldIn, FieldOut }
    public Type type;

    public GameObject player;
    BossManager boss;
    public GameObject enemyBoss;
    public GameObject hp_UI;
    public Transform spawnPos_Boss;
    public RectTransform hpBar;

    bool isReady;
    public bool isFight = false;
    public Vector3 spawnPos;
    void Update()
    {
        if (Input.GetButtonDown("Interation"))
        {
            if (type == Type.FieldIn && isReady)
            {
                player.transform.position = spawnPos;
                isFight = true;
                BossZone_IN();
            }
            else if (type == Type.FieldOut && isReady)
            {
                player.transform.position = spawnPos;
                isFight=false;
                BossZone_OUT();
            }
        }

        if (hp_UI.activeSelf == true)
        {
            hpBar.localScale = new Vector3(boss.curHp / boss.maxHp, 1, 1);
        }
    }

    void BossZone_IN()
    {
        Instantiate(enemyBoss,spawnPos_Boss.position, enemyBoss.transform.rotation);
        hp_UI.gameObject.SetActive(true);
    }
    void BossZone_OUT()
    {
        Destroy(enemyBoss);
        hp_UI.gameObject.SetActive(false);
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isReady = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isReady = false;
        }
    }
}
