using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBarView : MonoBehaviour
{
    Enemy enemyInfo;
    Slider hpSlider;

    public void Setup(Enemy enemy)
    {
        this.enemyInfo = enemy;
        hpSlider = GetComponent<Slider>();
    }
    void Awake()
    {
        enemyInfo = GetComponent<Enemy>();
        hpSlider = GetComponent<Slider>();
    }
    public void Update()
    {
        hpSlider.value = (float)enemyInfo.curHp / (float)enemyInfo.maxHp;
    }
}
