/*
 작성자 : 서재웅
 날짜 : 2023 - 12 - 07
 기능
// GameManager는 게임 관리와 씬 간의 전환을 담당하며, 플레이어 상태, 미션 및 스테이지 게이트, 보상 처리 등을 관리합니다.
// 각 스테이지에 따라 플레이어 캐릭터를 생성하고, 스테이지 진행 상황을 감지하여 게이트의 활성화 상태를 변경합니다.
// 미션 완료 여부를 추적하고, 플레이어 사망 시 상태를 초기화하고 패널을 표시하여 게임 상태를 관리합니다.
 */

using Cinemachine;
using UnityEngine;
using XEntity.InventoryItemSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GameManager>();
            }

            return m_instance;
        }
    }

    private static GameManager m_instance;

    [Header("Game Setting")]
    public GameObject menuCam;
    public GameObject gameCam;
    public PlayerM player;
    public Weapons weapon;
    Interactor interactor;
    public AOD aod;
    public bool isStart = false;
    public int stageNum;
    public GameObject warriorPrefeb;
    public GameObject archorPrefeb;
    public Transform startPoint;

    [Header("Player Base State")]
    [SerializeField] int baseHealth = 100;
    [SerializeField] int baseMaxHealth = 100;
    [SerializeField] int baseMp = 100;
    [SerializeField] int baseMaxMp = 100;
    [SerializeField] float baseSpeed = 10.0f;
    [SerializeField] int baseDamage = 20;
    [SerializeField] float baseDefence = 10.0f;

    [Header("Mission Gate")]
    public GameObject other1Door;
    public GameObject other2Door;
    public GameObject other3Door;
    public GameObject[] stage1Monster;
    public GameObject[] stage2Monster;
    public GameObject[] stage3Monster;
    public GameObject jumpMissionText;
    public Fire[] fireMissions;
    public int num = 0;
    public bool missionClear = false;

    [Header("Stage Gate")]
    public GameObject[] stage1;
    public GameObject[] stage2;
    public GameObject[] stage3;
    public GameObject[] stage4;
    public GameObject[] stage5;
    public GameObject[] stage6;
    public GameObject[] stage7;
    public GameObject[] stage8;
    public GameObject[] stage9;
    public GameObject[] stage10;
    public GameObject[] stage11;
    public GameObject[] stage12;
    public GameObject[] stage13;
    public GameObject[] stage_Boss;

    [Header("Misssion Reward")]
    public GameObject box;
    public GameObject Tp;

    void Awake()
    {
        isStart = true;
        this.enabled = true;
        StageCreateCharacter();
    }

    void StageCreateCharacter()
    {
        if (stageNum == 1 || stageNum == 2)
        {
            int selectNum = 0;
            if (stageNum == 1 && selectNum == 0)
            {
                GameObject warrior = Instantiate(warriorPrefeb);
                warrior.transform.position = new Vector3(0, 0.11f, 0);
                selectNum++;
            }
            if (stageNum == 2 & selectNum == 0)
            {
                GameObject archor = Instantiate(archorPrefeb);
                archor.transform.position = new Vector3(0, 0.11f, 0);
                selectNum++;
            }
        }
    }

    void Found()
    {
        player = FindObjectOfType<PlayerM>();
        weapon = FindObjectOfType<Weapons>();
        aod = FindObjectOfType<AOD>();
        interactor = player.GetComponent<Interactor>();
    }

    private void Start()
    {
        Found();
        Camera[] cams = FindObjectsOfType<Camera>();
        for (int i = 0; i < 2; i++)
        {
            if (cams[i].tag == "MainCamera")
            {
                Debug.Log("Find Cam");
                player.cam = cams[i];
                interactor.mainCamera = cams[i];
            }
        }
        player.virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if (stageNum == 3)
        {
            int test1 = 0;
            int test2 = 0;
            int test3 = 0;
            // ----------------------MisisonStage1----------------------
            for (int i = 0; i < stage1Monster.Length; i++)
            {
                if (stage1Monster[i] == null)
                {
                    test1++;
                }
                else
                {
                    test1 += 0;
                }
            }
            if (test1 >= stage1Monster.Length)
            {
                other1Door.SetActive(true);
            }
            // ----------------------MisisonStage2----------------------
            for (int i = 0; i < stage2Monster.Length; i++)
            {
                if (stage2Monster[i] == null)
                {
                    test2++;
                }
                else
                {
                    test2 += 0;
                }
            }
            if (test2 >= stage2Monster.Length)
            {
                other2Door.SetActive(true);
            }
            // ----------------------MisisonStage3----------------------
            for (int i = 0; i < stage3Monster.Length; i++)
            {
                if (stage3Monster[i] == null)
                {
                    test3++;
                }
                else
                {
                    test3 += 0;
                }
            }
            if (test3 >= stage3Monster.Length)
            {
                other3Door.SetActive(true);
            }
        }
    }

    public void Mission()
    {
        if (!missionClear)
        {
            if (num == fireMissions.Length - 1)
            {
                missionClear = true;
                box.SetActive(true);
                Tp.SetActive(true);
                Debug.Log("Mission Clear");
            }
        }
    }

    public void PlayerDie()
    {
        player.isDead = true;
        player.anim.SetTrigger("doDie");
        player.health = 0;
        player.cap.enabled = false;
        player.rb.useGravity = false;
        player.rb.velocity = Vector3.zero;
        aod.playPanel.SetActive(false);
        aod.diePanel.SetActive(true);
        PlayerStateReset();
    }

    public void PlayerStateReset()
    {
        player.maxHealth = baseMaxHealth;
        player.health = baseHealth;
        player.maxMP = baseMaxMp;
        player.speed = baseSpeed;
        player.defence = baseDefence;
        player.weapon.damage = baseDamage;
    }
}
