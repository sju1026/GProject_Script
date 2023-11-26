using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : MBullet
{
    public Transform target;
    NavMeshAgent nav;
    Rigidbody rb;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>(); 
    }

    private void Start()
    {
        rb.useGravity = false;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        StartCoroutine(BulletFire());
    }

    IEnumerator BulletFire()
    {
        nav.SetDestination(target.position);
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
