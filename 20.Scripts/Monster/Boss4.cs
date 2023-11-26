using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss4 : MonoBehaviour
{
    public enum Type { Cactus, Dragon, Wolf };

    [Header("EnemyState")]
    public Type bossType;
    public int maxHealth;
    public int curHealth;
    public BoxCollider meleeArea;
    public GameObject bullet;
    public GameObject[] itemPrefab;
    public bool isDead = false;
    public float defence;

    [Header("Player Chase")]
    public Transform target;
    public bool isChase;
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
    public GameObject nextGate;
    public bool isLook;

    Vector3 lookVec;
    Vector3 tauntVec;

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
        nav.isStopped = true;
        isFound = false;
        isLook = false;
    }

    void ChaseStart()
    {
        isChase = true;
        nav.isStopped = false;
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
            nextGate.SetActive(true);
            return;
        }

    }

    #region Attack Method
    void FreezeVeloCity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void Targerting()
    {
        float targetRadius = chaseRange;
        float targetRange = chaseRange * 5;

        RaycastHit[] raycastHits =
            Physics.SphereCastAll(transform.position,
                                    targetRadius,
                                    transform.forward,
                                    targetRange,
                                    LayerMask.GetMask("Player"));

        if (raycastHits.Length > 0 && !attackable)
        {
            StartCoroutine(Think());
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction = Random.Range(0, 4);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(Taunt());
                break;
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

        yield return new WaitForSeconds(2f);

        isChase = true;
        attackable = false;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        isChase = false;
        attackable = true;

        Vector3 tauntVec = new Vector3(target.position.x, target.position.y, target.position.z + 3);

        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1f);
        meleeArea.enabled = true;
        nav.SetDestination(tauntVec);

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(1f);
        isLook = true;

        isChase = true;
        attackable = false;

        StartCoroutine(Think());
    }
    #endregion

    void FixedUpdate()
    {
        Targerting();
        FreezeVeloCity();
    }

    public void DropItem()
    {
        int rand = Random.Range(0, 3);
        Vector3 spawnItem = new Vector3(transform.position.x, transform.position.y + 3.0f, transform.position.z);
        Instantiate(itemPrefab[rand], spawnItem, this.transform.rotation);
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
            Destroy(gameObject, 4);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFound = true;
            findSphere.enabled = false;
            ChaseStart();
            Debug.Log("succes");
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
            else if (other.tag == "Laser")
            {
                HS_EffectSound laser = other.GetComponent<HS_EffectSound>();
                curHealth -= laser.finaldamage;

                Vector3 reactVec = transform.position - other.transform.position;
                StartCoroutine(OnDamage(reactVec));
            }
        }
    }
}
