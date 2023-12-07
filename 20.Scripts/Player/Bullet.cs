/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// Bullet 클래스는 총알을 표현하며, 데미지와 이동속도, 회전량 등의 속성을 가집니다.
// 총알이 발사될 때 화면에 나타나는 효과와 충돌 시 특정 태그에 반응하여 오브젝트를 파괴합니다.
// 또한 총알의 회전 및 이동을 관리하고, 충돌한 대상이 "Enemy" 또는 "Wall" 태그인 경우 해당 오브젝트를 파괴합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    public float fireRate;
    public float rotateAmount = 45;
    public float speed;

    public GameObject muzzlePrefab;
    public GameObject hitPrefab;
    public List<GameObject> trails;

    private Vector3 offset;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();  // Getting the reference to the Rigidbody component.

        // Instantiate muzzle flash if the prefab is assigned.
        if (muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward + offset;  // Set muzzle direction.

            // Destroy the muzzle effect after its duration.
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
                Destroy(muzzleVFX, ps.main.duration);
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, rotateAmount, Space.Self);  // Rotate the object around its axis.

        // Move the object if speed is not zero and Rigidbody component is available.
        if (speed != 0 && rb != null)
            rb.position += (transform.forward + offset) * (speed * Time.deltaTime);
    }

    // OnTriggerEnter is called when the Collider other enters the trigger.
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is tagged as an "Enemy" or "Wall" and destroy the current object.
        if (other.tag == "Enemy" || other.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
