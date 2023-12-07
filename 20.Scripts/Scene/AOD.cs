/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// AOD 스크립트는 게임의 플레이어 상태와 UI, 게임 설정을 관리합니다.
// 플레이어 상태 및 능력치, 플레이 시간, UI 업데이트 및 게임 설정(일시정지 등)을 담당합니다.
// 게임의 플레이어 상태 정보 및 UI를 업데이트하고 게임 설정을 조작하는 기능을 제공합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AOD : MonoBehaviour
{
    PlayerM player;

    public GameObject playPanel;
    public GameObject diePanel;
    public GameObject pausePanel;
    public Button replayBtn;
    public Button exitBtn;

    [Header("Player State")]
    public Text[] texts;
    public RectTransform[] rects;
    public float playTime;
    public Text playTimeTxt;

    public Text damageTxt;
    public Text speedTxt;
    public Text depenceTxt;
    public Text attackRateTxt;

    public Text normalTxt;
    public Text skillTxt;
    public Text kickTxt;
    public Text boostTxt;

    public Text playerHealthTxt;
    public Text playerMpTxt;
    public RectTransform playerHealthBar;
    public RectTransform playerMPBar;

    [Header("Game Setting")]
    public bool isPause = false;


    private void Awake()
    {
        player = FindObjectOfType<PlayerM>();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (GameManager.instance.isStart)
        {
            playTime += Time.deltaTime;
        }
    }

    void LateUpdate()
    {
        // Timer
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int sec = (int)(playTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", sec);

        // Player State
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerMpTxt.text = player.mp + " / " + player.maxMP;

        playerHealthBar.localScale = new Vector3((float)player.health / player.maxHealth, 1, 1);
        playerMPBar.localScale = new Vector3((float)player.mp / player.maxMP, 1, 1);


        damageTxt.text = player.weapon.damage.ToString();
        speedTxt.text = player.speed.ToString();
        depenceTxt.text = player.defence.ToString();
        attackRateTxt.text = player.weapon.rate.ToString();

        normalTxt.text = player.normalLevel.ToString();
        skillTxt.text = player.skillLevel.ToString();
        kickTxt.text = player.kickLevel.ToString();
        boostTxt.text = player.boostLevel.ToString();

        // CheckMonster();

    }

    public void Replay()
    {
        Time.timeScale = 1;
        diePanel.SetActive(false);
        playPanel.SetActive(false);
        Destroy(player.gameObject);
        SceneManager.LoadScene(0);
        player.isDead = false;
        pausePanel.SetActive(false);

        GameGuide text = FindObjectOfType<GameGuide>();
        text.guideText.SetActive(false);

        GameManager.instance.PlayerStateReset();
    }

    public void ExitGame()
    {
        diePanel.SetActive(false);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Continue()
    {
        Time.timeScale = 1;
        isPause = false;
        pausePanel.SetActive(false);
    }
}
