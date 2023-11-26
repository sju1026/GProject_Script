using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        NormalSkill, Skill, KickSkill, BoostSkill, Helmet, Armor, Glove, Shose ,Weapon, StageClearKey, MP,
    };
    public Type type;
    public float value;

    Rigidbody rigid;
    public SphereCollider sphere;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 20f * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
        }
    }
}
