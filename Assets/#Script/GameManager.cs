using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    bool pauseDown;
    bool menuActivated = false;

    public Slider hpBar;
    public Text hpText;
    public Text potionCnt_Text;
    public Text ammoCnt_Text;
    public Text coin_Text;
    public Slider expBar;
    public Text leveUp_Text;

    public GameObject pauseMenu;
    public GameObject successSave;
    public GameObject gameOver;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image grenadeImg;

    void Update()
    {
        playerInfo();
        GetInput();
        GetPauseMenu();
        if (Information.stats.curExp >= Information.stats.maxExp) { GetLevelUp(); }
    }

    void LateUpdate()
    {
        weapon1Img.color = new Color(1,1,1,Information.stats.hasWeapons[0]?1:0);
        weapon2Img.color = new Color(1,1,1,Information.stats.hasWeapons[1]?1:0);
        weapon3Img.color = new Color(1,1,1,Information.stats.hasWeapons[2]?1:0);
        grenadeImg.color = new Color(1,1,1,Information.stats.hasGrenades>0?1:0);
    }
    void playerInfo()
    {
        expBar.value = (float)Information.stats.curExp / (float)Information.stats.maxExp;
        hpBar.value = (float)Information.stats.curHp / (float)Information.stats.maxHp;
        hpText.text = $"{Information.stats.curHp}/{Information.stats.maxHp}";
        potionCnt_Text.text = $"{Information.stats.potionCnt}";
        coin_Text.text = string.Format("{0:n0}",Information.stats.coin);
        ammoCnt_Text.text = string.Format("{0:n0}", $"{Information.stats.ammo} / {Information.stats.maxAmmo}" );
    }

    void GetInput()
    {
        pauseDown = Input.GetButtonDown("Pause");
    }

    void GetPauseMenu()
    {
        if (pauseDown)
        {
            menuActivated = !menuActivated;

            if (menuActivated) { GetOpenMenu(); }
            else { GetCloseMenu(); }
        }
    }
    void GetOpenMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
    void GetCloseMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void GetResume()
    {
        Time.timeScale = 1;
        menuActivated = false;
        pauseMenu.SetActive(false);
    }

    public void GetSave()
    {

        // 레벨, 경험치, 코인, 총알, 포션
        PlayerPrefs.SetInt("level", Information.stats.level);
        PlayerPrefs.SetInt("curExp", Information.stats.curExp);
        PlayerPrefs.SetInt("coin", Information.stats.coin);
        PlayerPrefs.SetInt("ammo", Information.stats.ammo);
        PlayerPrefs.SetInt("potion", Information.stats.potionCnt);
        PlayerPrefs.Save();

        Time.timeScale = 1;
        menuActivated = false;
        pauseMenu.SetActive(false);

        successSave.SetActive(true);
    }

    void GetLevelUp()
    {
        leveUp_Text.gameObject.SetActive(true);
        Information.stats.level++;
        Information.stats.curExp = 0;
    }

    public void GetMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameMenuScene");
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
    }
}
