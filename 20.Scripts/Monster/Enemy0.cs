using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;    

public class Enemy0 : MonoBehaviour
{
    
    public enum Type { A, B, C, D};

    [Header("EnemyState")]
    public Type enemyType;
    public int maxHealth;
    public int curHealth;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public Transform missilePortA;
    public Transform missilePortB;
    public GameObject missile;
    public GameObject[] itemPrefab;
    public bool isDead = false;
    public float defence;
    public float speed;
    bool isFound = false;
    public int dropcnt = 0;
    bool isBorder = false;

    [Header("Player Chase")]
    public Transform target;
    public bool isChase = false;
    public bool attackable = false;
    public float chaseRange = 3.0f;

    Rigidbody rigid;
    Material mat;
    NavMeshAgent nav;
    Animator anim;
    BoxCollider box;
    SphereCollider findSphere;

    void Awake()
    {
        target = FindObjectOfType<PlayerM>().transform;
        rigid = GetComponentInChildren<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        anim = GetComponentInChildren<Animator>();
        box = GetComponentInChildren<BoxCollider>();
        findSphere = GetComponent<SphereCollider>();
    }

    private void Start()
    {
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

    }

    void FreezeVeloCity()
    {
        if (isChase == false) {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        if (!isDead)
        {
            float targetRadius = 0;

            switch (enemyType)
            {
                case Type.A:
                    targetRadius = chaseRange;

                    break;
                case Type.B:
                    targetRadius = chaseRange / 3;

                    break;
                case Type.C:
                    targetRadius = chaseRange;

                    break;

                case Type.D:
                    targetRadius = chaseRange;

                    break;
            }

            Debug.DrawRay(transform.position, transform.forward * chaseRange, Color.red);
            Debug.DrawRay(transform.position, transform.right * chaseRange, Color.red);
            Debug.DrawRay(transform.position, -transform.forward * chaseRange, Color.red);
            Debug.DrawRay(transform.position, -transform.right * chaseRange, Color.red);

            RaycastHit[] rayHits =
                Physics.SphereCastAll(transform.position,
                                      targetRadius,
                                      transform.forward,
                                      targetRadius,
                                      LayerMask.GetMask("Player"));
            if (rayHits.Length > 0 && !attackable)
            {
                StartCoroutine(Attack());
            }
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

    IEnumerator Attack()
    {
        isChase = false;
        attackable = true;
        anim.SetBool("isAttack", true);

        switch (enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.7f);
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1.0f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 10, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(1.0f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet = Instantiate(missile, missilePortA.position, missilePortA.rotation);
                BossMissile bossMissile = instantBullet.GetComponent<BossMissile>();
                
                bossMissile.target = target;

                yield return new WaitForSeconds(1.0f);
                break;

            case Type.D:
                yield return new WaitForSeconds(0.2f);
                GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
                BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
                bossMissileA.target = target;

                yield return new WaitForSeconds(0.3f);
                GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
                BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
                bossMissileB.target = target;

                yield return new WaitForSeconds(0.5f);
                break;
        }

        anim.SetBool("isAttack", false);
        isChase = true;
        attackable = false;
    }
 
    public void DropItem()
    {
        if (dropcnt == 0) 
        {
            int luck = Random.Range(0, 2);
            if (luck == 1)
            {
                int rand = Random.Range(0, itemPrefab.Length);
                Vector3 spawnItem = new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z);
                Instantiate(itemPrefab[rand], spawnItem, this.transform.rotation);
                dropcnt++;
            }
            else
                Debug.Log("Fall");
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0)
        {
            mat.color = Color.white; 
        }
        else
        {
            box.enabled = false; 
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
            Destroy(gameObject, 1);      
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
