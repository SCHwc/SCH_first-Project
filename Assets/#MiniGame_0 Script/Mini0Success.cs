using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mini0Success : MonoBehaviour
{
    public GameObject Finish_UI;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Time.timeScale = 0;
            Finish_UI.SetActive(true);
            Information.stats.coin += 10000;
        }
    }
}
