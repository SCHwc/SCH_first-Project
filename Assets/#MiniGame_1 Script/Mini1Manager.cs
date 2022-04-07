using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mini1Manager : MonoBehaviour
{
    public static Mini1Manager mini1;
    public GameObject Finish_UI;
    public Text coin_text;
    public int coin_score;
    public int temp;
    void Awake()
    {
        temp = Information.stats.coin;
        coin_score = 0;
    }
    void Update()
    {
     if (Finish_UI.activeSelf == true)
        {
            GetScore();
        }
    }
    public void GetScore()
    {
        coin_score = Information.stats.coin - temp;
        coin_text.text = $"»πµÊ«— ƒ⁄¿Œ : {coin_score.ToString()}";
    }
    public void GetRetry()
    {
        Finish_UI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GetExit()
    {
        Finish_UI.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene("WaitingRoom");
    }
}
