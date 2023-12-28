/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// AllBoss 클래스는 여러 종류의 보스를 관리하며, 플레이어를 추적하고 공격하는 기능을 담당합니다.
// 보스의 종류에 따라 다양한 공격 패턴을 실행하고, 플레이어와의 상호작용으로 피해를 입거나 아이템을 드랍합니다.
// 또한 보스가 죽으면 해당 보스의 씬 클리어 및 다음 위치로 이동하는 동작을 수행합니다.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;     

public class AllBoss : MonoBehaviour
{
    public enum Type { Cactus, Dragon, Wolf};

    [Header("EnemyState")]
    public Type bossType;
    public int maxHealth;
    public int curHealth;
    public GameObject meleeArea;
    public GameObject[] itemPrefab;
    public bool isDead = false;
    public float defence;
    public int dropcnt = 0;
    public float speed;
    bool isFound = false;
    bool isBorder = false;
    public GameObject clearBox;
    public GameObject nextTP;

    [Header("Player Chase")]
    public Transform target;
    public bool isChase = false;
    public bool attackable = false;
    public float chaseRange;

    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public Material mat;
    public NavMeshAgent nav;
    public Animator anim;
    SphereCollider findSphere;

    [Header("Boss State")]
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;

    void Start()
    {
        target = FindObjectOfType<PlayerM>().transform;
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        anim = GetComponentInChildren<Animator>();
        findSphere = GetComponent<SphereCollider>();

        nav.speed = 0;
    }

    void ChaseStart()
    {
        isChase = true;
        nav.speed = speed;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (nav.enabled && target != null && !isBorder)
        {
            nav.SetDestination(target.position);
        }

        if (attackable == true)
        {
            nav.isStopped = true;
        }
        else if (attackable == false)
            nav.isStopped = false;

        if (isDead)
        {
            StopAllCoroutines();
            clearBox.SetActive(true);
            nextTP.SetActive(true);
            return;
        }
        
    }

    #region Attack Method
    void FreezeVeloCity()
    {
        if (isChase == false)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        float targetRadius = chaseRange;

        Debug.DrawRay(transform.position, transform.forward * chaseRange, Color.red);
        Debug.DrawRay(transform.position, transform.right * chaseRange, Color.red);
        Debug.DrawRay(transform.position, -transform.forward * chaseRange, Color.red);
        Debug.DrawRay(transform.position, -transform.right * chaseRange, Color.red);

        RaycastHit[] raycastHits =
            Physics.SphereCastAll(transform.position,
                                    targetRadius,
                                    transform.forward,
                                    targetRadius,
                                    LayerMask.GetMask("Player"));

        if (raycastHits.Length > 0 && !attackable)
        {
            Think();
        }
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }

    void FixedUpdate()
    {
        StopToWall();
        Targerting();
        FreezeVeloCity();
    }

    void Think()
    {
        int index = Random.Range(0, 3);
        if (index == 0)
        {
            float temp = chaseRange;
            chaseRange = 1.0f;
            StartCoroutine(Taunt());
            chaseRange = temp;
        }
        else
        {
            StartCoroutine(MissileShot());
        }
    }

    IEnumerator MissileShot()
    {
        isChase = false;
        attackable = true;

        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        if (bossType.ToString() == "Dragon")
        {
            yield return new WaitForSeconds(0.3f);
            GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
            BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
            bossMissileB.target = target;
        }
        yield return new WaitForSeconds(0.5f);

        isChase = true;
        attackable = false;
    }

    IEnumerator Taunt()
    {
        isChase = false;
        attackable = true;

        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(0.2f);
        meleeArea.SetActive(true);

        yield return new WaitForSeconds(1.3f);
        meleeArea.SetActive(false);
        yield return new WaitForSeconds(0.5f);

        isChase = true;
        attackable = false;

    }
    #endregion

    public void DropItem()
    {
        if (dropcnt == 0)
        {
            int rand = Random.Range(0, itemPrefab.Length);
            Vector3 spawnItem = new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z);
            Instantiate(itemPrefab[rand], spawnItem, transform.rotation);
            dropcnt++;
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            mat.color = Color.white; 
        }
        else
        {
            boxCollider.enabled = false;
            rigid.useGravity = false;
            mat.color = Color.gray;
            Debug.Log("Dead");
            isDead = true;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");
            DropItem();

            reactVec = reactVec.normalized;
            reactVec += Vector3.up;
            rigid.AddForce(reactVec * 5, ForceMode.Impulse); 
            Destroy(gameObject, 2);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFound = true;
            findSphere.enabled = false;
            ChaseStart();
        }
        if (isFound)
        {
            if (other.tag == "Weapon")
            {
                Weapons weapon = other.GetComponent<Weapons>();
                curHealth -= weapon.damage - (int)defence / weapon.damage;
                Vector3 reactVec = transform.position - other.transform.position;

                StartCoroutine(OnDamage(reactVec));
            }
            else if (other.tag == "Bullet")
            {
                Bullet bullet = other.GetComponent<Bullet>();
                curHealth -= bullet.damage;
                Vector3 reactVec = transform.position - other.transform.position;
                Destroy(other.gameObject);
                StartCoroutine(OnDamage(reactVec));
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isFound)
        {
            if (other.tag == "Laser")
            {
                HS_EffectSound laser = other.GetComponent<HS_EffectSound>();
                curHealth -= laser.finaldamage;

                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
            }
        }
    }
}
