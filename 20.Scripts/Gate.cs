/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// Gate는 게임 스테이지 간의 이동을 관리하며, 게이트 열고 닫기, 스테이지 클리어 감지를 수행합니다.
// 스테이지 클리어 상태에 따라 게이트를 열거나 닫아 플레이어의 이동을 제어하며, 특정 이벤트 발생 시 게이트 동작을 처리합니다.
// 게이트의 이동 및 활성화 여부를 체크하여 플레이어와 상호작용하며, 게임 플레이에 필요한 스테이지 간 이동을 제어합니다.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject[] stageList;
    public GameObject otherGate;

    public PlayerM player;
    public BoxCollider box;

    public float gateMoveValue;
    public int gateValue; // 0-> Start / 20->Boss
    public int testnum = 0;

    private void Start()
    {
        player = FindObjectOfType<PlayerM>();
        // box = GetComponent<BoxCollider>();

        box.enabled = false;
        GateImport();
    }

    private void LateUpdate()
    {
        StageClear();
    }

    public void GateImport()
    {
        if (gateValue == 1)
        {
            stageList = GameManager.instance.stage1;
        }
        else if (gateValue == 2)
        {
            stageList = GameManager.instance.stage2;
        }
        else if (gateValue == 3)
        {
            stageList = GameManager.instance.stage3;
        }
        else if (gateValue == 4)
        {
            stageList = GameManager.instance.stage4;
        }
        else if (gateValue == 5)
        {
            stageList = GameManager.instance.stage5;
        }
        else if (gateValue == 6)
        {
            stageList = GameManager.instance.stage6;
        }
        else if (gateValue == 7)
        {
            stageList = GameManager.instance.stage7;
        }
        else if (gateValue == 8)
        {
            stageList = GameManager.instance.stage8;
        }
        else if (gateValue == 9)
        {
            stageList = GameManager.instance.stage9;
        }
        else if (gateValue == 10)
        {
            stageList = GameManager.instance.stage10;
        }
        else if (gateValue == 11)
        {
            stageList = GameManager.instance.stage11;
        }
        else if (gateValue == 12)
        {
            stageList = GameManager.instance.stage12;
        }
        else if (gateValue == 13)
        {
            stageList = GameManager.instance.stage13;
        }
        else if (gateValue == 20)
            stageList = GameManager.instance.stage_Boss;
    }

    public void GateClose()
    {
        // gameObject.SetActive(true);

        float x = transform.position.x;
        float y = transform.position.y - gateMoveValue;
        float z = transform.position.z;
        
        transform.position = new Vector3(x, y, z);

        float x1 = otherGate.transform.position.x;
        float y1 = otherGate.transform.position.y - gateMoveValue;
        float z1 = otherGate.transform.position.z;

        otherGate.transform.position = new Vector3(x1, y1, z1);
    }

    public void GateOpen()
    {
        // gameObject.SetActive(false);

        float x = transform.position.x;
        float y = transform.position.y + gateMoveValue;
        float z = transform.position.z;

        float x1 = otherGate.transform.position.x;
        float y1 = otherGate.transform.position.y + gateMoveValue;
        float z1 = otherGate.transform.position.z;

        transform.position = new Vector3(x, y, z);
        otherGate.transform.position = new Vector3(x1, y1, z1);
    }

    public void StageClear()
    {
        for (int i = 0; i < stageList.Length; i++)
        {
            if (stageList[i] == null)
            {
                testnum++;
            }
            else
            {
                testnum += 0;
            }
        }
        if (testnum >= stageList.Length)
        {
            if (gateValue == 20 && player.stageClear_Key_Num == 1)
            {
                box.enabled = true;
            }
            else
            {
                box.enabled = true;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player" && box.enabled == true)
        {
            GateOpen();
            box.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && box.enabled == true)
        {
            StartCoroutine(Close());
        }
    }

    IEnumerator Close()
    {
        yield return new WaitForSeconds(0.5f);
        GateClose();
    }
}
