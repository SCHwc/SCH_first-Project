using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartUI : MonoBehaviour
{
    void Awake()
    {
    }

    void Update()
    {

    }

    public void NewGame()
    {
        SceneManager.LoadScene("WaitingRoom");
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("level")) { return; }

        int curExp = PlayerPrefs.GetInt("curExp");
        int lev = PlayerPrefs.GetInt("level");
        int coin = PlayerPrefs.GetInt("coin");
        int ammo = PlayerPrefs.GetInt("ammo");
        int potion = PlayerPrefs.GetInt("potion");

        SceneManager.LoadScene("WaitingRoom");

        Information.stats.level = lev;
        Information.stats.curExp = curExp;
        Information.stats.coin = coin;
        Information.stats.ammo = ammo;
        Information.stats.potionCnt = potion;

    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
