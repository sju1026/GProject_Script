/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// ItemBox 클래스는 상자와 상호작용하여 아이템을 생성하고, 플레이어와 상호작용하여 아이템을 랜덤하게 배치합니다.
// 상자와 플레이어 간의 상호작용으로 랜덤하게 아이템을 생성하며, 생성되는 아이템의 종류는 상자의 속성에 따라 다르게 결정됩니다.
// 일반, 희귀, 희소, 에픽 아이템을 배치하고, 상자에 상호작용할 시 랜덤한 아이템을 생성하여 게임 플레이에 영향을 줍니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    public GameObject[] normalItem;
    public GameObject[] rareItem;
    public GameObject[] epicItem;
    public GameObject key;
    public Transform[] spawnPoint;

    BoxCollider boxCollider;

    public PlayerM player;
    public int boxValue;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
        player = FindObjectOfType<PlayerM>();
    }

    public void RandomSpawn()
    {
        if(boxValue == 1)
        {
            int index = Random.Range(0, normalItem.Length);
            int transform = Random.Range(0, spawnPoint.Length);
            Instantiate(normalItem[index], spawnPoint[transform].transform.position, spawnPoint[transform].transform.rotation);
        }
        else if (boxValue == 2)
        {
            int index = Random.Range(0, rareItem.Length);
            int transform = Random.Range(0, spawnPoint.Length);
            Instantiate(rareItem[index], spawnPoint[transform].transform.position, spawnPoint[transform].transform.rotation);
        }
        else if (boxValue == 3)
        {
            int index = Random.Range(0, normalItem.Length);
            int transform = Random.Range(0, spawnPoint.Length);
            int luck = Random.Range(0, 5);
            if(luck == 0)
            {
                Instantiate(epicItem[index], spawnPoint[transform].transform.position, spawnPoint[transform].transform.rotation);
            }
            else
            {
                Instantiate(rareItem[index], spawnPoint[transform].transform.position, spawnPoint[transform].transform.rotation);
            }
            Instantiate(key, spawnPoint[3].position, spawnPoint[3].rotation);
        }
        else if (boxValue == 4)
        {
            int index = Random.Range(0, epicItem.Length);
            int transform = Random.Range(0, spawnPoint.Length);
            Instantiate(epicItem[index], spawnPoint[transform].transform.position, spawnPoint[transform].transform.rotation);
        }

        Destroy(gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            if (player.iDown)
            {
                Debug.Log("PlayerInteraction");
                RandomSpawn();
            }
        }
    }
}
