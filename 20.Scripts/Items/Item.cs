/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// Item 클래스는 게임 아이템을 정의하고, 아이템의 종류와 가치를 설정합니다.
// 아이템 회전, 바닥 충돌 감지, 아이템 유형 및 값을 설정하는 물리적 동작을 관리합니다.
// 아이템의 회전 및 바닥 충돌로부터 물리 효과를 처리하고, 아이템의 유형 및 가치를 정의합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        NormalSkill, Skill, KickSkill, BoostSkill, Helmet, Armor, Glove, Shose ,Weapon, StageClearKey, MP, HP,
    };
    public Type type;
    public float value;

    Rigidbody rigid;
    public SphereCollider sphere;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphere = GetComponent<SphereCollider>();
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
