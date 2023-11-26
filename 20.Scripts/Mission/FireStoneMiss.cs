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

            Debug.LogWarning("�ֶ��� �߹�");

            if (first.isLight == true && second.isLight == true || second.isLight == true && first.isLight == true)
            {
                Debug.LogWarning("���� �޽�");
                first.isLight = false;
                second.isLight = false;

                GameManager.instance.Mission();
            }
            else if (first.isLight == true && second.isLight == false || second.isLight == false && first.isLight == true)
            {
                Debug.LogWarning("�ϳ� Ʈ��");
                first.isLight = false;
                second.isLight = true;

                GameManager.instance.Mission();
            }
            else if (first.isLight == false && second.isLight == true || second.isLight == true && first.isLight == false)
            {
                Debug.LogWarning("�ϳ� Ʈ��");
                first.isLight = true;
                second.isLight = false;

                GameManager.instance.Mission();
            }
            else if (first.isLight == false && second.isLight == false || second.isLight == false && first.isLight == false)
            {
                Debug.LogWarning("���� Ʈ��");
                first.isLight = true;
                second.isLight = true;

                GameManager.instance.Mission();
            }

        }
    }
}
