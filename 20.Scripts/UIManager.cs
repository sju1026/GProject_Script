using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject mainPannel;
    public GameObject characterPannel;
    public GameObject recordPannel;
    public GameObject exitPannel;
    public AOD aod;

    public void CharacterPannel()
    {
        mainPannel.SetActive(false);
        characterPannel.SetActive(true);
    }

    public void WarriorSelect()
    {
        SceneManager.LoadScene("S_Desert_Ver1");
        AOD aod = FindObjectOfType<AOD>();
        aod.playPanel.SetActive(true);
    }
    public void ArchorSelect()
    {
        SceneManager.LoadScene("S_Desert_Ver2");
        AOD aod = FindObjectOfType<AOD>();
        aod.playPanel.SetActive(true);
    }
}
