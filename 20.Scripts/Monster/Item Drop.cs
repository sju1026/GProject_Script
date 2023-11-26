using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public GameObject[] items = new GameObject[3];
    // ������ �Ӽ��� 3���� ���� ������Ʈ �迭�� 3���� ���� 
    // Use this for initialization
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
            // �ڷ�ƾ�� ����ϵ��� �Ѵ� :D

        }
    }

    IEnumerator dropTheItems()
    {
        int maxItems = 10;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < maxItems; i++)
        {
            int rand = Random.Range(0, 3);
            // �������� �����մϴ�(0 ~2����)
            yield return new WaitForSeconds(0.3f); // �����̸� ����ϴ�.
            Instantiate(items[rand], trans.position, Quaternion.identity);
            // ���ľ���
            // �������� ���� �ڸ��� ��ȯ�մϴ�.
        }
        Destroy(this.gameObject);

        // ���� �� �� ��ũ��Ʈ�� ���̻� �����ϸ� �ȵǱ� ������ �ı��Ѵ�.
    }
}
