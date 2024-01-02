/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// LoadingSceneController는 씬 전환과 함께 로딩 화면을 관리하며, 다음 씬으로의 전환 및 로딩 진행도를 표시합니다.
// 씬을 비동기적으로 로드하고, 로딩 프로세스를 진행하면서 진행 막대의 채움 정도를 조절합니다.
// 로딩이 거의 완료된 경우 일정 시간 후 다음 씬으로 전환하고 필요한 오브젝트들의 활성화를 조절합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingSceneController : MonoBehaviour
{
    static string nextScene;
    public static bool startScene;

    public Image background;

    public Sprite[] sprites;

    [SerializeField]
    Image progressBar;

    PlayerM player;
    AOD aod;

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }
    void Start()
    {
        if (startScene == false)
        {
            player = FindObjectOfType<PlayerM>();
            aod = FindObjectOfType<AOD>();
        }
        StartCoroutine(LoadSceneProcess());
    }

    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op =  SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        int randomSpriteNum = Random.Range(0, sprites.Length);
        background.sprite = sprites[randomSpriteNum];

        player.rb.useGravity = false;
        player.gameObject.SetActive(false);
        player.cam = null;
        player.virtualCam = null;
        aod.playPanel.SetActive(false);

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;

            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = op.progress;
            }
            else
            {
                timer += Time.unscaledTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
                if(progressBar.fillAmount >= 1f)
                {
                    yield return new WaitForSeconds(5.0f);
                    op.allowSceneActivation = true;
                   
                    player.gameObject.SetActive(true);
                    player.rb.useGravity = true;
                    aod.playPanel.SetActive(true);
                    
                    yield break;
                }
            }
        }
    }
}
