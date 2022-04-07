using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Information : MonoBehaviour
{
    public static Information stats;

    public int coin;
    public int maxCoin = 100000;
    public int ammo = 0;
    public int maxAmmo = 9999;
    public int potionCnt;
    public int maxPotionCnt = 100;

    public int level = 1;

    public int curExp = 0;
    public int maxExp = 100;

    public int curHp;
    public int maxHp;
    public int itemHp;

    public bool[] hasWeapons;
    public Weapon equipWeapon;
    public int equipWeaponIndex = -1;

    public int HandGun_curAmmo = 12;
    public int SubGun_curAmmo = 25;
    public int hasGrenades = 0;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (stats == null) { stats = this; }
        else if (stats != this) { Destroy(gameObject); }

        maxExp = level * 100;
        maxHp = (level * 50) + 50 + itemHp;
        curHp = maxHp;
    }

    void Update()
    {
        GetStats();
    }

    void GetStats()
    {
        maxExp = level * 100;
        maxHp = (level * 50) + 50 + itemHp;
        if (potionCnt >= maxPotionCnt)
        {
            potionCnt = maxPotionCnt;
        }
    }
}
