using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Animator animator;
    Rigidbody rigid;

    public Status status;

    [SerializeField]
    ParticleSystem LevelUpEffect;
    public ParticleSystem GetLevelUpEffect() { return LevelUpEffect; }

    StatusMarket NPC;

    PlayerSKillSystem[] CurrentSkill;
    PlayerUI ui;
    
    static float WalkSpeed = 2f;
    static float RunSpeed = 4f;
    float RotSpeed = 5.0f;

    string currentState = "Idle";
    bool isWalking = false;
    bool isRun = false;
    bool IsAttackState = false;
    public bool GetIsAttackState() { return IsAttackState; }
    bool isDead = false;
    public bool GetIsDead() { return isDead; }
    bool IsBuffPlay = false;

    static int PlayerLevel;
    public int GetPlayerLevel() { return PlayerLevel; }
    public int SetPlayerLevel() { return PlayerLevel++; }

    static int MaxHealth;
    static int MaxMana;
    public int GetMaxHealth() { return MaxHealth; }
    public int IncreaseMaxHealth(int value) { return MaxHealth = MaxHealth + value; }
    public int GetMaxMana() { return MaxMana; }
    public int IncreaseMaxMana(int value) { return MaxMana = MaxMana + value; }

    private int LevelUPExp = 1000;
    public int GetLevelUPExp() { return LevelUPExp; }
    public int SetLevelUPExp()
    {
        int temp = LevelUPExp / 5;
        return LevelUPExp = LevelUPExp + temp;
    }

    int PlayerExp;
    public int GetPlayerExp() { return PlayerExp; }
    public int SetPlayerExp(int value) { return PlayerExp = value;}

    public int IncreaseExp(int value) { return PlayerExp = PlayerExp + value;}
    public int DecreaseExp(int value) { return PlayerExp = PlayerExp - value;}

    float AttackTime = 0.0f;
    float SpendTime = 0.0f;

    float NPCDistance = 1.2f;

    bool IsPlayerMove = false;
    public bool GetIsPlayerMove() { return IsPlayerMove; }
    public bool SetIsPlayerMove(bool value) { return IsPlayerMove = value;}


    UserData LoginData;
    public UserData GetUserData() { return LoginData; }
    IEnumerator Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rigid = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        ui = GetComponent<PlayerUI>();

        LoginData = Singleton.Instance.GetUserData();
        PlayerLevel = LoginData.UserLevel;

        StartSetting();
        GameObject[] TempSkill = GameObject.FindGameObjectsWithTag("PlayerSkill");
        SkillSet[] SkillList = new SkillSet[TempSkill.Length];
        for(int i = 0;i<TempSkill.Length;i++)
        {
            SkillList[i] =  TempSkill[i].transform.GetChild(0).GetComponent<PlayerSKillSystem>().GetThisSkillData();
        }

        NPC = GameObject.FindGameObjectWithTag("NPC").GetComponent<StatusMarket>();

        CurrentSkill = GameObject.Find("PlayerSkill").GetComponentsInChildren<PlayerSKillSystem>();

        while (Application.isPlaying)
        {
            yield return StartCoroutine(currentState);
        }
    }

    private void StartSetting()
    {
        status.SetHealth(100 + ((PlayerLevel - 1) * 10));
        status.SetMana(100 + ((PlayerLevel - 1) * 5));
        status.SetAttackDamage(10 + ((PlayerLevel - 1) * 2));
        status.SetDefence(0 + (PlayerLevel - 1));
        status.SetMoveSpeed(2);

        MaxHealth = status.GetHealth();
        MaxMana = status.GetMana();
    }

    IEnumerator FadeOut()
    {
        Image Fade = GameObject.Find("FadeOut").GetComponent<Image>();

        if(Fade.color.a < 1.0f)
        {
            float Alpha = Fade.color.a;
            Fade.color = new Color(0.0f, 0.0f, 0.0f, Alpha + (Time.deltaTime / 2));
        }
        else
        {
            SceneManager.LoadScene("Map_v2");
        }

        yield return null;

    }

    IEnumerator FadeIn()
    {
        Image Fade = GameObject.Find("FadeOut").GetComponent<Image>();

        if (Fade.color.a > 0.0f)
        {
            float Alpha = Fade.color.a;
            Fade.color = new Color(0.0f, 0.0f, 0.0f, Alpha - (Time.deltaTime / 2));
        }
        else
        {
            IsFadeIn = true;
            StopCoroutine("FadeIn");
        }

        yield return null;
    }

    bool IsFadeIn = false;
    float DeadDelay = 5.0f;
    void Update()
    {
        if (!IsFadeIn)
            StartCoroutine("FadeIn");

        if (status.GetHealth() <= 0)
        {
            isDead = true;
            IsPlayerMove = false;
            animator.Play("Dead");

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

            if (DeadDelay >= 0)
                DeadDelay -= Time.deltaTime;
            else
            {
                Singleton.Instance.SetNowLevel(PlayerLevel);
                StartCoroutine(FadeOut());
            }
        }

        PlayerSkillUI SkillUI = GetComponent<PlayerSkillUI>();
        if (IsPlayerMove == false && Input.GetKeyDown(KeyCode.K))
        {
            IsPlayerMove = true;
            SkillUI.SlideInOut(IsPlayerMove);
        }
        else if(IsPlayerMove == true && Input.GetKeyDown(KeyCode.K))
        {
            IsPlayerMove = false;
            SkillUI.SlideInOut(IsPlayerMove);
        }

        if(Input.GetKeyDown(KeyCode.Return) && !GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().GetIsStagePlaying())
        {
            GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().NextStage();
        }

        if (!isDead)
        {
            if (IsPlayerMove == false)
                Move();

            if (status.GetMana() < GetMaxMana())
            {
                SpendTime += Time.deltaTime;
                if (SpendTime > 0.5f)
                {
                    status.IncreaseMana(1); 
                    SpendTime = 0.0f;
                }
            }
        }

        float dist = Vector3.Distance(this.transform.position, NPC.transform.position);
        if (dist < NPCDistance && Input.GetKeyDown(KeyCode.E))
        {
            NPC.SetNPC_UIState();

            isWalking = false;
            animator.SetBool("isWalking", false);

            status.SetMoveSpeed(WalkSpeed);
            isRun = false;
            animator.SetBool("isRun", false);
        }

         ui.SetUI();
            
    }

    private void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 MoveDir = (Vector3.forward * v) + (Vector3.right * h);

        if (h == 0 && v == 0)
        {
            isWalking = false;
            animator.SetBool("isWalking", false);
        }
        else
        {
            isWalking = true;
            animator.SetBool("isWalking", true);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            status.SetMoveSpeed(RunSpeed);
            isRun = true;
            animator.SetBool("isRun", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            status.SetMoveSpeed(WalkSpeed);
            isRun = false;
            animator.SetBool("isRun", false);
        }

        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * RotSpeed, 0));
        transform.Translate(MoveDir.normalized * status.GetMoveSpeed() * Time.deltaTime);
    }

    public void SetAttackDamage()
    {
        status.IncreaseAttackDamage(2);
    }

    public void SetDefenceUP()
    {
        status.IncreaseDefence(1);
    }

    IEnumerator Idle()
    {
        if (IsPlayerMove == false)
        {
            animator.Play("New State");
            if (status.GetMana() >= 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    IsAttackState = true;
                    currentState = "Attack1";
                    yield break;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    IsAttackState = true;
                    currentState = "Skill1";
                    yield break;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    IsAttackState = true;
                    currentState = "Skill2";
                    yield break;
                }
            }
        }
    }

    IEnumerator Attack1()
    {
        animator.Play("Attack1");
        AttackTime += Time.deltaTime;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);

        if (AttackTime < info.length)
        {
            if (Input.GetMouseButtonDown(0) && AttackTime > info.length - 0.2f)
            {
                status.SetMana(status.DecreaseMana(4));
                AttackTime = 0.0f;
                currentState = "Attack2";
                yield break;
            }
        }
        else if(AttackTime > info.length)
        {
            status.SetMana(status.DecreaseMana(4));
            AttackTime = 0.0f;
            IsAttackState = false;
            currentState = "Idle";
        }
    }

    IEnumerator Attack2()
    {
        animator.Play("Attack2");
        AttackTime += Time.deltaTime;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);

        if (AttackTime > info.length)
        {
            status.SetMana(status.DecreaseMana(3));
            AttackTime = 0.0f;
            IsAttackState = false;
            currentState = "Idle";
            yield break;
        }
    }

    IEnumerator Skill1()
    {
        if (CurrentSkill[0].SkillData.IsSkillActivate &&
            CurrentSkill[0].SkillData.IsReadySkill)
        {
            animator.Play(CurrentSkill[0].SkillData.ClipName);

            AttackTime += Time.deltaTime;

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);

            if (AttackTime < info.length)
            {
                yield break;
            }
            else
            {
                CurrentSkill[0].SkillData.IsReadySkill = false;
                status.DecreaseMana(CurrentSkill[0].SkillData.NeedMana);
                AttackTime = 0.0f;
                currentState = "Idle";
            }

            if (CurrentSkill[0].SkillData.SkillEffect != null)
            {
                Instantiate(CurrentSkill[0].SkillData.SkillEffect, transform);
                CurrentSkill[0].SkillData.SkillEffect.Play();

                BuffState(CurrentSkill[0].SkillData.Name);
            }
        }
        else
        {
            IsAttackState = false;
            currentState = "Idle";
        }
    }

    IEnumerator Skill2()
    {
        if (CurrentSkill[1].SkillData.IsSkillActivate &&
            CurrentSkill[1].SkillData.IsReadySkill)
        {
            animator.Play(CurrentSkill[1].SkillData.ClipName);

            AttackTime += Time.deltaTime;

            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(1);

            if (AttackTime < info.length)
            {
                yield break;
            }
            else
            {
                CurrentSkill[1].SkillData.IsReadySkill = false;
                status.DecreaseMana(CurrentSkill[1].SkillData.NeedMana);
                AttackTime = 0.0f;
                currentState = "Idle";
            }

            if (CurrentSkill[1].SkillData.SkillEffect != null)
            {
                Instantiate(CurrentSkill[1].SkillData.SkillEffect, transform);
                CurrentSkill[1].SkillData.SkillEffect.Play();

                BuffState(CurrentSkill[1].SkillData.Name);
            }
        }
        else
        {
            IsAttackState = false;
            currentState = "Idle";
        }
    }

    void BuffState(string name)
    {
        switch(name)
        {
            case "AttackUP":
                status.SetAttackDamage(status.GetAttackDamage() * 2);

                StartCoroutine(BuffAttackUP(5));
                break;
            case "Berserker":
                status.SetAttackDamage(status.GetAttackDamage() * 2);
                status.SetDefence(status.GetDefence() * 2);
                status.SetMoveSpeed(WalkSpeed * 2);
                RunSpeed = status.GetMoveSpeed() * 2;

                StartCoroutine(BuffBerserker(5));
                break;
        }
    }

    IEnumerator BuffAttackUP(float time)
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        status.SetAttackDamage(status.GetAttackDamage()/2);
        yield return null;
    }

    float TempTime = 1.0f;
    IEnumerator BuffBerserker(float time)
    {
        while (time > 0)
        {
            if (TempTime < 0)
            {
                status.DecreaseHealth(PlayerLevel);
                TempTime = 1.0f;
            }
            else
                TempTime -= Time.deltaTime;

            time -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        status.SetAttackDamage(status.GetAttackDamage()/2);
        status.SetDefence(status.GetDefence()/2);
        status.SetMoveSpeed(WalkSpeed);

        RunSpeed = WalkSpeed * 2;
        yield return null;
    }
}
