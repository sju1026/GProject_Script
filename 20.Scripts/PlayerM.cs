using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class PlayerM : MonoBehaviour
{
    [Header("Player State")]
    public float speed;
    public float plusSpeed;
    public int health;
    public int maxHealth;
    public int mp;
    public int maxMP;
    public float defence;
    public float plusDefence;
    public bool isDead;
    public int stageClear_Key_Num = 0;


    [Header("Player Skill Enforce")]
    public int normalnum;
    public int skillnum;
    public int kickSkillnum;
    public int boostSkillnum;

    public int normalLevel;
    public int skillLevel;
    public int kickLevel;
    public int boostLevel;

    [Header("Cam")]
    public Camera followCamera;
    float hAxis;
    float vAxis;


    [Header("Player KeySetting")]
    #region InputButton
    public bool fDown;
    public bool f2Down;
    public bool bDown;
    public bool kDown;
    public bool iDown;
    public bool finalDown;
    bool jDown;
    bool wDown;
    public bool isJump;
    public bool isDodge;
    public bool isBorder;
    #endregion

    [Header("Player Object")]
    public GameObject boostObject;
    public GameObject finalAttack;

    Vector3 moveVec;
    Vector3 dodgeVec;

    public CapsuleCollider cap;
    public Rigidbody rb;
    public Animator anim;
    public ItemBox box;
    public Weapons weapon;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        cap = GetComponent<CapsuleCollider>();
        weapon = GetComponentInChildren<Weapons>();

        if (!isDead)
        {
            maxHealth = 100;
            health = maxHealth;
            maxMP = 100;
            speed = 15.0f;
            mp = maxMP;
            defence = 10.0f;

            weapon.isBoost = true;
        }
    }

    void Update()
    {
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Boost();
        FinalAttack();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal"); // W/D
        vAxis = Input.GetAxisRaw("Vertical"); // A/D
        wDown = Input.GetButton("Walk"); // Left Shift
        jDown = Input.GetButtonDown("Jump"); // SpaceBar
        fDown = Input.GetButtonDown("Fire1"); // Left Click
        f2Down = Input.GetButtonDown("Fire2"); // Q
        bDown = Input.GetButtonDown("Skill1"); // E
        kDown = Input.GetButtonDown("Kick"); // C
        iDown = Input.GetButton("Interaction"); // F
        finalDown = Input.GetButtonDown("FinalAttack"); // R
    }

    #region Movement
    void Move()
    {
        moveVec = new Vector3(hAxis, 0f, vAxis).normalized;

        if (isDodge)
            moveVec = dodgeVec;

        if (!isBorder && !isDead)
        {
            transform.position += moveVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;
        }

        anim.SetBool("isRun", moveVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        transform.LookAt(transform.position + moveVec);

        if (fDown || f2Down || kDown && !isDead)
        {
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
    }

    void Jump()
    {
        if (jDown && moveVec == Vector3.zero && !isJump && !isDodge && !isDead)
        {
            rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
            isJump = true;
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

        }
    }

    void Dodge()
    {
        if (jDown && moveVec != Vector3.zero && !isJump && !isDodge && !isDead)
        {
            dodgeVec = moveVec;
            speed *= 2;
            anim.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }

    void FreezeRotation()
    {
        if (weapon.isAttack)
        {
            rb.velocity = Vector3.zero;
        }
        rb.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        Debug.DrawRay(transform.position, transform.forward * 3, Color.green);
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }

    private void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
    }
    #endregion

    #region Skills
    void Boost()
    {
        if (bDown && moveVec == Vector3.zero && mp >=weapon.skillcostMp && !isJump && !isDodge && !isDead && weapon.isBoost)
        {
            Debug.Log("BOOST");
            mp -= weapon.skillcostMp;
            weapon.isBoost = false;
            weapon.boostSkillCoolTimeText.text = "";

            anim.SetTrigger("doBoost");
            StartCoroutine(Boosting());

            StartCoroutine(weapon.BoostResetSkillCoroutine());
            StartCoroutine(weapon.BoostCoolTimeCountCoroutine(weapon.boostSkillCoolTime));
        }
    }

    IEnumerator Boosting()
    {
        yield return new WaitForSeconds(0.1f);
        boostObject.SetActive(true);
        float temp = speed;
        speed += 2.5f;

        yield return new WaitForSeconds(10.0f);
        boostObject.SetActive(false);
        speed = temp;
    }

    void FinalAttack()
    {
        if (finalDown && moveVec == Vector3.zero && mp >= weapon.finalCostMp && !isJump && !isDodge && !isDead && weapon.isFinal)
        {
            Debug.Log("FinalAttack");
            mp -= weapon.finalCostMp;
            weapon.isFinal = false;
            weapon.FinalSkillCoolTimeText.text = "";

            anim.SetTrigger("doFinalAttack");
            StartCoroutine(FinalSkill());

            StartCoroutine(weapon.FinalAttackResetSkillCoroutine());
            StartCoroutine(weapon.FinalCoolTimeCountCoroutine(weapon.FinalSkillCoolTime));
        }
    }

    IEnumerator FinalSkill()
    {
        yield return new WaitForSeconds(0.5f);

        finalAttack.SetActive(true);

        yield return new WaitForSeconds(2.0f);
        finalAttack.SetActive(false);

    }
    #endregion

    #region SkillLevelUp
    public void NormalLevelUp()
    {
        if (normalnum > 0 && normalLevel <= 3)
        {
            normalLevel++;
            normalnum--;
        }
    }
    public void SkillLevelUp()
    {
        if (skillnum > 0 && skillLevel <= 3)
        {
            skillLevel++;
            skillnum--;
        }
    }
    public void KickLevelUp()
    {
        if (kickSkillnum > 0 && kickLevel <= 3)
        {
            kickLevel++;
            kickSkillnum--;
        }
    }
    public void BoostLevelUp()
    {
        if (boostSkillnum > 0 && boostLevel <= 3)
        {
            boostLevel++;
            boostSkillnum--;
        }
    }
    #endregion


    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyBullet")
        {
            MBullet enemydam = other.GetComponent<MBullet>();
            health -= enemydam.damage - (int)defence / enemydam.damage;

            if (health <= 0)
            {
                health = 0;
                isDead = true;
                anim.SetTrigger("doDie");
                cap.enabled = false;
                rb.useGravity = false;
            }
     
        }

        if (other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.NormalSkill:
                    normalnum++;
                    item.sphere.enabled = false;
                    break;
                case Item.Type.Skill:
                    skillnum++;
                    item.sphere.enabled = false;
                    break;
                case Item.Type.KickSkill:
                    kickSkillnum++;
                    item.sphere.enabled = false;
                    break;
                case Item.Type.BoostSkill:
                    boostSkillnum++;
                    item.sphere.enabled = false;
                    break;
                case Item.Type.StageClearKey:
                    stageClear_Key_Num++;
                    item.sphere.enabled = false;
                    break;
                case Item.Type.MP:
                    if (mp + (int)item.value >= 100)
                    {
                        mp = 100;
                    }
                    else if(mp < 100)
                    {
                        mp += (int)item.value;
                    }
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
        }
    }

}
