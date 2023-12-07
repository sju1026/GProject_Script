using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStoneMiss : MonoBehaviour
{
    //public GameObject[] sprites;
    public GameObject test1;
    public GameObject test2;

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Weapon" || other.tag == "Bullet") 
        {

            Fire first = test1.GetComponent<Fire>();
            Fire second = test2.GetComponent<Fire>();

            if (first.isLight == true && second.isLight == true || second.isLight == true && first.isLight == true)
            {
                first.isLight = false;
                second.isLight = false;
                GameManager.instance.num -= 2;
            }
            else if (first.isLight == true && second.isLight == false || second.isLight == false && first.isLight == true)
            {
                first.isLight = false;
                second.isLight = true;
            }
            else if (first.isLight == false && second.isLight == true || second.isLight == true && first.isLight == false)
            {
                first.isLight = true;
                second.isLight = false;
            }
            else if (first.isLight == false && second.isLight == false || second.isLight == false && first.isLight == false)
            {
                first.isLight = true;
                second.isLight = true;
                GameManager.instance.num += 2;
            }
            GameManager.instance.Mission();
        }
    }
}
