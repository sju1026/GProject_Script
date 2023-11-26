using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public int nextsceneValue;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene(nextsceneValue);
            PlayerM player = other.GetComponent<PlayerM>();
            player.stageClear_Key_Num = 0;
        }
    }
}
