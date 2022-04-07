using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject[] item;
    public int[] iPrice;
    public GameObject shopMenuUI;

    bool inShop;
    bool menuActivated = false;


    void Awake()
    {
    }

    void Update()
    {
    }

    public void GetItemBuy(int index)
    {

        int price = iPrice[index];

        if (price > Information.stats.coin) { return; }
        Information.stats.coin -= price;

        switch (index)
        {
            case 0:
                Information.stats.ammo += 30;
                break;
            case 1:
                Information.stats.itemHp += 10;
                break;
            case 2:
                Information.stats.hasGrenades++;
                break;
        }
    }

    public void GetWeaponBuy(int index)
    {
        int price = iPrice[index];

        if (price > Information.stats.coin) { return; }
        if (Information.stats.hasWeapons[index]) { return; }
        Information.stats.coin -= price;

        Information.stats.hasWeapons[index] = true;
    }


    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            inShop = true;

            if (inShop)
            {
                GetShopMenu();
            }
        }

    }

    void GetShopMenu()
    {
        if (Input.GetButtonDown("Interation"))
        {
            menuActivated = !menuActivated;

            if (menuActivated) { GetOpenMenu(); }
            else { GetCloseMenu(); }
        }
    }
    void GetOpenMenu()
    {
        Time.timeScale = 0;
        shopMenuUI.SetActive(true);
    }
    public void GetCloseMenu()
    {
        Time.timeScale = 1;
        shopMenuUI.SetActive(false);
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            inShop = false;
        }
    }


}
