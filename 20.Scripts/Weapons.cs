using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    
    public enum Type { Sword, Bow}

    [Header("Weapon State")]
    public Type type;
    public int damage = 1000;
    public int skillLevelUpDamage;
    public float rate;
    public bool isAttack;
    public bool kick = false;
    public int skillcostMp = 10;
    public int finalCostMp = 50;
    public float skillCoolTime = 2.0f;
    public float boostSkillCoolTime = 10.0f;
    public float FinalSkillCoolTime = 60.0f;
    public Image skillFillAmount;
    public Text skillCoolTimeText;
    public Image boostSkillFillAmount;
    public Text boostSkillCoolTimeText;
    public Image FinalSkillFillAmount;
    public Text FinalSkillCoolTimeText;

    bool isSkill;
    public bool isBoost;
    public bool isFinal;

    [Header("Weapon Components")]
    PlayerM player;
    public BoxCollider swordArea;
    public BoxCollider leftKickArea;
    public BoxCollider rightKickArea;
    public TrailRenderer trailEffect;
    public Transform bulletPos;
    public GameObject bullet;
    public GameObject bullet2;

    private void Start()
    {
        player = GetComponentInParent<PlayerM>();

        damage = 1000;
        isAttack = false;
        isSkill = true;
        isBoost = true;
        isFinal = true;

        skillFillAmount.fillAmount = 0;
        boostSkillFillAmount.fillAmount = 0;
        FinalSkillFillAmount.fillAmount = 0;
    }

    public void Use()
    {

        if (player.fDown && type == Type.Sword && !isAttack)
        {
            isAttack = true;
            StopCoroutine(Swing());
            StartCoroutine(Swing());
        }
        else if (player.f2Down && type == Type.Sword && isSkill && !isAttack)
        {
            if (player.mp >= skillcostMp) {
                player.mp -= skillcostMp;
                isAttack = true;
                isSkill = false;

                skillCoolTimeText.text = "";

                StopCoroutine(StrongSwing());
                StartCoroutine(StrongSwing());

                StartCoroutine(ResetSkillCoroutine());
                StartCoroutine(CoolTimeCountCoroutine(skillCoolTime));
            }
            else
            {
                Debug.Log("No Mp");
            }

        }
        else if (player.fDown && type == Type.Bow && !isAttack)
        {
            isAttack = true;
            StartCoroutine(Shot());
        }

        else if (player.f2Down && type == Type.Bow && isSkill && !isAttack)
        {
            if (player.mp >= skillcostMp)
            {
                player.mp -= skillcostMp;
                isAttack = true;
                isSkill = false;

                skillCoolTimeText.text = "";

                StartCoroutine(StrongShot());

                StartCoroutine(ResetSkillCoroutine());
                StartCoroutine(CoolTimeCountCoroutine(skillCoolTime));
            }
            else
            {
                Debug.Log("No Mp");
            }

        }
        else if (kick && !isAttack)
        {
            isAttack = true;
            StopCoroutine(Kick());
            StartCoroutine(Kick());
        }
    }

    IEnumerator ResetSkillCoroutine()
    {
        while (skillFillAmount.fillAmount < 1)
        {
            skillFillAmount.fillAmount += 1 * Time.smoothDeltaTime / skillCoolTime;

            yield return null;
        }

        isSkill = true;
    }
    public IEnumerator BoostResetSkillCoroutine() 
    {
        while (boostSkillFillAmount.fillAmount < 1)
        {
            boostSkillFillAmount.fillAmount += 1 * Time.smoothDeltaTime / boostSkillCoolTime;

            yield return null;
        }

        isBoost = true;
    }

    public IEnumerator FinalAttackResetSkillCoroutine()
    {
        while (FinalSkillFillAmount.fillAmount < 1)
        {
            FinalSkillFillAmount.fillAmount += 1 * Time.smoothDeltaTime / FinalSkillCoolTime;

            yield return null;
        }

        isFinal = true;
    }

    public IEnumerator CoolTimeCountCoroutine(float number)
    {
        if (number > 0)
        {
            number -= 1;

            if (!isSkill)
                skillCoolTimeText.text = number.ToString();
            else if (!isBoost)
                boostSkillCoolTimeText.text = number.ToString();

            yield return new WaitForSeconds(1f);
            StartCoroutine(CoolTimeCountCoroutine(number));
        }
        else
        {
            if (isSkill)
            {
                skillCoolTimeText.text = "";
                skillFillAmount.fillAmount = 0;
            }

            if (isBoost)
            {
                boostSkillCoolTimeText.text = "";
                boostSkillFillAmount.fillAmount = 0;
            }
            yield break;
        }
    }

    public IEnumerator BoostCoolTimeCountCoroutine(float number)
    {
        if (number > 0)
        {
            number -= 1;

            if (!isBoost)
                boostSkillCoolTimeText.text = number.ToString();

            yield return new WaitForSeconds(1f);
            StartCoroutine(BoostCoolTimeCountCoroutine(number));
        }
        else
        {
            if (isBoost)
            {
                boostSkillCoolTimeText.text = "";
                boostSkillFillAmount.fillAmount = 0;
            }
            yield break;
        }
    }

    public IEnumerator FinalCoolTimeCountCoroutine(float number)
    {
        if (number > 0)
        {
            number -= 1;

            if (!isFinal)
                FinalSkillCoolTimeText.text = number.ToString();

            yield return new WaitForSeconds(1f);
            StartCoroutine(FinalCoolTimeCountCoroutine(number));
        }
        else
        {
            if (isFinal)
            {
                FinalSkillCoolTimeText.text = "";
                FinalSkillFillAmount.fillAmount = 0;
            }
            yield break;
        }
    }

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        skillLevelUpDamage = damage + (player.normalLevel * 5);
        int tempD = damage;
        damage = skillLevelUpDamage;

        float temp = player.speed;
        player.speed = 0;

        swordArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(1.0f);
        swordArea.enabled = false;
        trailEffect.enabled = false;

        player.speed = temp;
        damage = tempD;
        skillLevelUpDamage = 0;
        isAttack = false;
        isSkill = false;
    }

    IEnumerator StrongSwing()
    {
        yield return new WaitForSeconds(0.1f);
        skillLevelUpDamage = damage + (player.skillLevel * 10);
        int tempD = damage;
        damage = skillLevelUpDamage;

        float temp = player.speed;
        player.speed = 0;

        swordArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.5f);
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;

        yield return new WaitForSeconds(0.5f);
        swordArea.enabled = false;
        trailEffect.enabled = false;

        player.speed = temp;
        damage = tempD;
        skillLevelUpDamage = 0;
        isAttack = false;
    }

    IEnumerator Shot()
    {
        yield return new WaitForSeconds(0.1f);
        skillLevelUpDamage = damage + (player.normalLevel * 5);
        int tempD = damage;
        damage = skillLevelUpDamage;

        float temp = player.speed;
        player.speed = 0;

        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;

        yield return new WaitForSeconds(1.0f);

        trailEffect.enabled = false;

        damage = tempD;
        skillLevelUpDamage = 0;
        player.speed = temp;
        isAttack = false;
        yield return null;
    }

    IEnumerator StrongShot()
    {
        yield return new WaitForSeconds(0.1f);
        skillLevelUpDamage = damage + (player.skillLevel * 10);
        int tempD = damage;
        damage = skillLevelUpDamage;

        float temp = player.speed;
        player.speed = 0;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.3f);

        GameObject instantBullet = Instantiate(bullet2, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRb = instantBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = bulletPos.forward * 50;

        yield return new WaitForSeconds(1.0f);

        trailEffect.enabled = false;

        damage = tempD;
        skillLevelUpDamage = 0;
        player.speed = temp;
        isAttack = false;
        yield return null;
    }

    IEnumerator Kick()
    {
        yield return new WaitForSeconds(0.1f);
        skillLevelUpDamage = damage + (player.kickLevel * 3);
        int tempD = damage;
        damage = skillLevelUpDamage;

        float temp = player.speed;
        player.speed = 0;

        leftKickArea.enabled = true;

        yield return new WaitForSeconds(0.8f);

        leftKickArea.enabled = false;
        rightKickArea.enabled = true;

        yield return new WaitForSeconds(0.8f);

        rightKickArea.enabled = false;

        damage = tempD;
        skillLevelUpDamage = 0;
        player.speed = temp;
        isAttack = false;
    }
}
