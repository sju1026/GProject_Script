using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string nextSceneName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (GameManager.instance.stageNum == 4)
            {
                AOD aod = FindObjectOfType<AOD>();
                aod.clearPanel.SetActive(true);
                aod.playPanel.SetActive(false);
            }
            LoadingSceneController.LoadScene(nextSceneName);
            PlayerM player = other.GetComponent<PlayerM>();
            player.stageClear_Key_Num = 0;
            player.transform.position = new Vector3(0, 0.11f, 0);
        }
    }
}
