using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System;

public enum State
{
    Idle = 0,
    Move = 1,
    Attack = 2,
    Death = 3
}

[Serializable]
public class EnemyController : MonoBehaviour
{
    public Status status;
    Animator Anim;
    State EnemyState;
    public State GetState() { return EnemyState; }

    NavMeshAgent nav;
    SphereCollider[] AIMove;
    int ColliderCount = 0;
    
    GameObject Player;
    private float DetectedDistance = 5f;
    public float AttackDistance = 2f;

    float Distance;
    public GameObject prefap;
    Canvas HPCanvas;
    GameObject HpBar;
    Slider slider;

    CapsuleCollider Col;
    BoxCollider boxColl;

    float DeathTime = 5f;


    static int MaxHP;
    bool IsDeath = false;

    bool IsAttack = false;
    public bool GetIsAttack() { return IsAttack; }

    bool IsEnemyAttackState = false;
    public bool GetIsEnemyAttackState() { return IsEnemyAttackState; }
    public int Exp;

    protected virtual void Awake()
    {
        Anim = GetComponent<Animator>();
        EnemyState = State.Idle;

        Player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();

        if (status.GetMoveSpeed() != 0)
            nav.speed = status.GetMoveSpeed();
    }

    public void SetEnemyActivate()
    {
        IsDeath = false;
        nav.enabled = true;

        status.SetHealth(100 + (GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().GetStageCount() *10));
        MaxHP = status.GetHealth();
        status.SetAttackDamage(status.GetAttackDamage() + (GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().GetStageCount() * 2));
        status.SetDefence(1 + (GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().GetStageCount() * 1));

        SetState(State.Idle);
        NextState();
    }

    void Start()
    {
        HPCanvas = gameObject.GetComponentInChildren<Canvas>();
        HpBar = Instantiate<GameObject>(prefap,HPCanvas.transform);
        slider = HpBar.GetComponent<Slider>();
        SetHpBar();

        Col = GetComponent<CapsuleCollider>();
        if (Col == null)
            boxColl = GetComponent<BoxCollider>();

        StartCoroutine(Idle());
    }

    public void NextState()
    {
        StartCoroutine(EnemyState.ToString());
    }

    public void SetState(State enemyState)
    {
        EnemyState = enemyState;
        Anim.SetInteger("CurrentState", (int)EnemyState);
    }

    private IEnumerator Idle()
    {
        Transform Parent = this.transform.parent.GetChild(0).transform;
        AIMove = Parent.GetComponentsInChildren<SphereCollider>();

        nav.SetDestination(AIMove[0].transform.position);
        SetHpBar();

        if (Col != null)
            Col.enabled = true;
        else
            boxColl.enabled = true;

        SetState(State.Move);
        yield return null;
        NextState();
    }

    private IEnumerator Move()
    {
        Distance = Vector3.Distance(this.transform.position, Player.transform.position);

        if(status.GetHealth() <= 0)
        {
            SetState(State.Death);
        }
        else if (Distance < DetectedDistance && Distance > AttackDistance)
        {
            nav.SetDestination(Player.transform.position);
        }
        else if(Distance < AttackDistance)
        {
            SetState(State.Attack);
        }
        else if(Distance > DetectedDistance)
        {
            nav.SetDestination(AIMove[ColliderCount].transform.position);
        }
        yield return null;

        NextState();
    }

    float CurrentTime;
    private IEnumerator Attack()
    {
        IsEnemyAttackState = true;
        transform.LookAt(Player.transform);
        Anim.SetInteger("CurrentState", (int)EnemyState);

        CurrentTime += Time.deltaTime;
        AnimatorStateInfo info = Anim.GetCurrentAnimatorStateInfo(1);

        if(CurrentTime > info.length)
        {
            CurrentTime = 0.0f;
            IsEnemyAttackState = false;

            SetState(State.Move);
        }

        yield return null;
        NextState();
    }

    private IEnumerator Death()
    {
        IsDeath = true;
        Anim.SetInteger("CurrentState", (int)EnemyState);
        Anim.SetTrigger("DeathTrigger");

        if (Col != null)
            Col.enabled = false;
        else
            boxColl.enabled = false;
        nav.enabled = false;

        Player.GetComponent<PlayerController>().IncreaseExp(Exp);

        yield return null;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MoveAI")
        {
            ColliderCount++;

            if (ColliderCount >= AIMove.Length) ColliderCount = 0;

            Vector3 dir = AIMove[ColliderCount].transform.position - this.transform.position;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime);
        }
    }

    void SetHpBar()
    {
        float Percent = (float)status.GetHealth() / (float)MaxHP;
        slider.value = Percent;
    }

    public int DecreaseHP(int Damage)
    {
        status.DecreaseHealth(Damage);
        SetHpBar();
        return status.GetHealth();
    }

    void Update()
    {
        if (IsDeath)
        {
            DeathTime -= Time.deltaTime;
            if (DeathTime <= 0f)
            {
                Singleton.Instance.SetKillCount();
                this.gameObject.SetActive(false);
                CreateItemInstance Test = GameObject.Find("DropItem").GetComponent<CreateItemInstance>();
                Test.SetRespawnItem(this);

                GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().ReCallEnemy(this);
                GameObject.Find("RespawnSpot").GetComponent<EnemyRespawn>().EnemyDeathCheck();

                DeathTime = 5.0f;
            }
        }

        if (Player.GetComponent<PlayerController>().GetIsDead())
        {
            SetState(State.Idle);
            StopAllCoroutines();
        }
    }

    public void PlayDamageAnim()
    {
        Anim.SetTrigger("DamageTrigger");
    }
}
