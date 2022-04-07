using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDie : MonoBehaviour
{
    public GameObject Obj;
    void Start()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Time.timeScale = 1;
            Obj.SetActive(false);
            SceneManager.LoadScene("WaitingRoom");
            Information.stats.curHp = Information.stats.maxHp;
        }
    }
}
