using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemSlot : MonoBehaviour
{
    public Item item;
    public int itemCnt;
    public Image itemimg;

    public Text text_cnt;
    public GameObject cntimg;

    void SetColor(float _alpha)
    {
        Color color = itemimg.color;
        color.a = _alpha;
        itemimg.color = color;
    }

    // 아이템 획득
    public void AddItem(Item _item, int _cnt = 1)
    {
        item = _item;
        itemCnt = _cnt;

        if (item.type != Item.Type.Weapon)
        {
            cntimg.SetActive(true);
            text_cnt.text = itemCnt.ToString();
        }
        else
        {
            cntimg.SetActive(false);
            text_cnt.text = "0";
        }

        SetColor(1);
    }

    // 아이템 갯수조정
    public void SetSlotCnt(int _cnt)
    {
        itemCnt += _cnt;
        text_cnt.text = itemCnt.ToString();

        if (itemCnt <= 0)
        {
            ClearSlot();
        }
    }

    // 아이템이 없을시 슬롯 초기화
    void ClearSlot()
    {
        item = null;
        itemCnt = 0;
        itemimg.sprite = null;
        SetColor(0);

        text_cnt.text = "0";
        cntimg.SetActive(false);
    }

    void Update()
    {

    }
}
