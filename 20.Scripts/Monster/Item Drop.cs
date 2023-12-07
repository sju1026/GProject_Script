using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] items = new GameObject[3];
    Transform trans;
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "item")
        {
            Vector3 v = new Vector3(0f, 5.0f, 1f);
            trans.position = other.transform.position + v;
            StartCoroutine("dropTheItems");

        }
    }

    IEnumerator dropTheItems()
    {
        int maxItems = 10;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < maxItems; i++)
        {
            int rand = Random.Range(0, 3);
            yield return new WaitForSeconds(0.3f);
            Instantiate(items[rand], trans.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
}
