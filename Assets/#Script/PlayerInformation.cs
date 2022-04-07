using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    bool inHouse;

    void Awake()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();

            switch (item.type)
            {
                case Item.Type.Coin:
                    Information.stats.coin += item.value;
                    if (Information.stats.coin > Information.stats.maxCoin)
                    {
                        Information.stats.coin = Information.stats.maxCoin;
                    }
                    break;

                case Item.Type.Ammo:
                    Information.stats.ammo += item.value;
                    if (Information.stats.ammo > Information.stats.maxAmmo)
                    {
                        Information.stats.ammo = Information.stats.maxAmmo;
                    }
                    break;

                case Item.Type.Potion:
                    Information.stats.potionCnt += item.value;
                    if (Information.stats.potionCnt > Information.stats.maxPotionCnt)
                    {
                        Information.stats.potionCnt = Information.stats.maxPotionCnt;
                    }
                    break;

            }

            Destroy(other.gameObject);
        }
    }

    
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "House")
        {
            inHouse = true;
        }

        if (inHouse)
        {
            if (Input.GetButtonDown("Interation"))
            {
                while (Information.stats.curHp < Information.stats.maxHp)
                {
                    Information.stats.curHp += 10;

                    if (Information.stats.curHp >= Information.stats.maxHp)
                    {
                        Information.stats.curHp = Information.stats.maxHp;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "House")
        {
            inHouse = false;
        }
    }
}
