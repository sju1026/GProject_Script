/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
 // 게임 내 전투에서 전사 캐릭터의 공격과 아이템 상호작용을 제어합니다.
 // 플레이어 입력에 따라 장착한 무기로의 공격을 관리하고, 아이템 획득 시 캐릭터 스탯을 수정합니다.
 // 특정 아이템을 획득하여 체력, 방어력, 이동 속도, 무기 데미지 등 캐릭터 속성을 업데이트합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : MonoBehaviour
{
    PlayerM pm;
    Weapons weapon;

    public GameObject[] weapons;
    public Weapons equipWeapon;
    public float fireDelay;

    public bool isFireReady = true;

    private void Start()
    {
        pm = GetComponent<PlayerM>();
        weapon = GetComponentInChildren<Weapons>();
    }

    void Update()
    {
        Attack();
    }

    void Attack()
    {
        if (equipWeapon == null)
            return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if (pm.fDown && isFireReady && !pm.isJump && !pm.isDodge && !pm.isDead)
        {
            equipWeapon.Use();
            if (equipWeapon.type == Weapons.Type.Sword || equipWeapon.type == Weapons.Type.Bow) // Base Attack
            {
                pm.anim.SetTrigger("doSwing");
            }
            fireDelay = 0;
        }

        if (pm.f2Down && isFireReady && !pm.isJump && !pm.isDodge && !pm.isDead)
        {
            equipWeapon.Use();
            if (equipWeapon.type == Weapons.Type.Sword || equipWeapon.type == Weapons.Type.Bow) // Skill
            {
                pm.anim.SetTrigger("doSwing2");
            }
            fireDelay = 0;
        }

        if (pm.kDown && isFireReady && !pm.isJump && !pm.isDodge && !pm.isDead)
        {
            equipWeapon.Use();
            equipWeapon.kick = true;
            if (equipWeapon.type == Weapons.Type.Sword || equipWeapon.type == Weapons.Type.Bow) // Kick
            {
                pm.anim.SetTrigger("doKick");
            }
            fireDelay = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.Helmet:
                    pm.maxHealth += 50 * (int)item.value;
                    pm.health += 50 * (int)item.value;
                    Destroy(other.gameObject);
                    break;
                case Item.Type.Armor:
                    pm.maxHealth += 50 * (int)item.value;
                    pm.health += 50 * (int)item.value;
                    pm.defence += 5 * (int)item.value;
                    Destroy(other.gameObject);
                    break;
                case Item.Type.Glove:
                    weapon.rate += (float)0.5 * item.value;
                    Destroy(other.gameObject);
                    break;
                case Item.Type.Shose:
                    pm.speed += (float)0.5 * item.value;
                    Destroy(other.gameObject);
                    break;
                case Item.Type.Weapon:
                    weapon.damage += 50 * (int)item.value;
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
        }
    }
}
