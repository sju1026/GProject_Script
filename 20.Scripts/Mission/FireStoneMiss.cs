using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStoneMiss : MonoBehaviour
{
    //public GameObject[] sprites;
    public GameObject test1;
    public GameObject test2;

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Bullet") 
        {

            Fire first = test1.GetComponent<Fire>();
            Fire second = test2.GetComponent<Fire>();

            Debug.LogWarning("왜때려 야발");

            if (first.isLight == true && second.isLight == true || second.isLight == true && first.isLight == true)
            {
                Debug.LogWarning("전부 펄스");
                first.isLight = false;
                second.isLight = false;

                GameManager.instance.Mission();
            }
            else if (first.isLight == true && second.isLight == false || second.isLight == false && first.isLight == true)
            {
                Debug.LogWarning("하나 트루");
                first.isLight = false;
                second.isLight = true;

                GameManager.instance.Mission();
            }
            else if (first.isLight == false && second.isLight == true || second.isLight == true && first.isLight == false)
            {
                Debug.LogWarning("하나 트루");
                first.isLight = true;
                second.isLight = false;

                GameManager.instance.Mission();
            }
            else if (first.isLight == false && second.isLight == false || second.isLight == false && first.isLight == false)
            {
                Debug.LogWarning("전부 트루");
                first.isLight = true;
                second.isLight = true;

                GameManager.instance.Mission();
            }

        }
    }
}
