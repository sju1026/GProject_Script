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
    public GameObject bullet;
    public GameObject[] itemPrefab;
    public bool isDead = false;
    public float defence;
    public int dropcnt = 0;
    public float speed;
    public GameObject clearBox;
    public GameObject nextTP;

    [Header("Player Chase")]
    public Transform target;
    public bool isChase = false;
    public bool attackable = false;
    public float chaseRange = 3.0f;
    public bool isFound;

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

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        mat = GetComponentInChildren<SkinnedMeshRenderer>().material;
        anim = GetComponentInChildren<Animator>();
        target = FindObjectOfType<PlayerM>().transform;
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
        if (nav.enabled && target != null)
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

    void FixedUpdate()
    {
        Targerting();
        FreezeVeloCity();
    }

    public void DropItem()
    {
        if (dropcnt == 0)
        {
            int rand = Random.Range(0, 3);
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
