using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public abstract class AttackStrategy
{
    public abstract void Attack();

}

public abstract class MonveStrategy
{
    public abstract void Monve();
}

//전략 패턴
public class MeleeAttackStrategy : AttackStrategy
{
    public override void Attack()
    {
        Debug.Log("근접공격한다");
    }

}
public class RangedAttackStrategy : AttackStrategy
{
    public override void Attack()
    {
        Debug.Log("원거리 공격 한다");
    }
}

public class MonsterMoveStrategy : MonveStrategy
{
    public override void Monve()
    {
        Debug.Log("플레이를 향해 이동한다");
    }
}

// 상태패턴
public abstract class StateMachine
{
    public abstract void EnterIdleState();
    public abstract void EnterChaseState();
    public abstract void EnterAttackState();

    public abstract void UpdateState();
}

public class MonsterStateMachine : StateMachine
{
    // 이패턴의 주인은 오너다.
    private Monster owner;

    //상태패턴
    public enum State
    {
        Idle,
        Chase,
        Attack
    }

    public State curruntState;

    //생성자를 통해서 초기화하고 초기화 값은 아이들이다.
    public MonsterStateMachine(Monster owner)
    {
        this.owner = owner;
        curruntState = State.Idle;
    }

    //상태 전환
    public override void EnterIdleState()
    {
        curruntState = State.Idle;
        Debug.Log("멍때리는중");
    }
    public override void EnterChaseState()
    {
        curruntState = State.Chase;
        Debug.Log("대상 추적");
    }

    public override void EnterAttackState()
    {
        curruntState = State.Attack;
        Debug.Log("대상 공격");
    }

    //FSM
    public override void UpdateState()
    {
        switch (curruntState)
        {
            case State.Idle:
                curruntState = State.Idle;
                UpdateStateIdel();
                break;
            case State.Chase:
                curruntState = State.Chase;
                UpdateStateChase();
                break;
            case State.Attack:
                curruntState = State.Attack;
                UpdateStateAttack();
                break;

        }
    }
    private void UpdateStateIdel()
    {
        // 타겟이 있으면 추적 상태로 전환
        if (owner.target != null)
        {

            float distanceToTarget = Vector3.Distance(owner.transform.position, owner.target.transform.position);
            // 타겟이 감지 범위 안에 들어오면
            if (distanceToTarget <= owner.detectionRange)
            {
                //추적상태로 전환
                EnterChaseState();
            }
        }
    }
    private void UpdateStateChase()
    {
        if (owner.target != null)
        {
            // 타겟까지의 방향 벡터 계산
            Vector3 direction = owner.target.transform.position - owner.transform.position;
            direction.y = 0; // Y축은 무시 (평면 이동)

            float distanceToTarget = direction.magnitude; // 거리 계산

            // 공격 범위 안에 들어오면 공격 상태로 전환
            if (distanceToTarget <= owner.attackRange)
            {
                EnterAttackState();
                return;
            }

            // 타겟을 향해 이동
            owner.transform.position += direction.normalized * owner.MoveSpeed * Time.deltaTime;

            // 타겟을 바라보도록 회전
            if (direction != Vector3.zero)
            {
                owner.transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            // 타겟이 없으면 대기 상태로 돌아감
            EnterIdleState();
        }
    }

    private void UpdateStateAttack()
    {
        if (owner.target != null)
        {
            float distanceToTarget = Vector3.Distance(owner.transform.position, owner.target.transform.position);
            owner.Attack();
            // 타겟이 공격 범위를 벗어나면 다시 추적
            if (distanceToTarget > owner.attackRange)
            {
                EnterChaseState();
                return;
            }
        }
        else
        {
            // 타겟이 없으면 대기 상태로
            EnterIdleState();
        }
    }

}





public class Monster : MonoBehaviour
{
    public GameObject target;
    MeleeAttackStrategy Ms;
    RangedAttackStrategy Rs;
    MonsterMoveStrategy Moves;
    ItemDeta itemDeta;
    //속성
    public AttackStrategy AS;
    [SerializeField]
    private int hp;
    [SerializeField]
    private float atk;
    [SerializeField]
    private float def;
    [SerializeField]
    private float moveSpeed;
    public bool isDead = false;

    protected StateMachine stateMachine;

    public float detectionRange = 10;
    public float attackRange = 2;
    public float attackCooldown = 2;
    [HideInInspector]
    public float attackCooldownTimer = 0;

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }
    public float Atk
    {
        get { return atk; }
        set { atk = value; }
    }

    public float Def
    {
        get { return def; }
        set { def = value; }
    }
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }
    //상태 선택

    public void Attack()
    {
        Ms = new MeleeAttackStrategy();
        Rs = new RangedAttackStrategy();

    }
    public void Move()
    {
        Moves = new MonsterMoveStrategy();
    }
    //기능
    public void Die()
    {
        if (hp <= 0)
        {
            hp = 0;
        }

        if (hp == 0 && !isDead)
        {
            isDead = true;

            Debug.Log("몬스터가 사망했습니다.");
            DropRandomItem();

            Destroy(gameObject, 2f);

        }
    }
    public void TakeDamage(float damage)
    {
        float damageTaken = damage - Def;
        //데미지 계산
        if (damageTaken < 0)
            damageTaken = 0;
        hp -= (int)damageTaken;
        Debug.Log("몬스터가 " + damageTaken + "의 피해를 입었습니다. 남은 HP: " + hp);
        Die();
    }

    protected virtual void Start()
    {
        //스테이트 머신 시작할때  몬스터의 상태를 넣고, 처음은아이들
        stateMachine = new MonsterStateMachine(this);
        stateMachine.EnterIdleState();

        //아이템들 초기화하고, 드랍아이템 있는지확인
        itemDeta = gameObject.AddComponent<ItemDeta>();
        itemDeta.DropItem();

        AS = new MeleeAttackStrategy();
        //타겟 찾기 태그로 플레이어 찾고, 비어있으면 타겟이라고 지정
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player;
        }
        Debug.Log(gameObject.name + " 몬스터 생성 HP: " + hp);
    }

    protected virtual void Update()
    {
        if (isDead) { return; }
        stateMachine?.UpdateState();
        // 테스트: T키를 누르면 10 데미지
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }

        // 테스트: Y키를 누르면 50 데미지
        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(50);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AS?.Attack();
        }
    }

    private void DropRandomItem()
    {
        // 아이템 리스트가 비어있는지 확인
        if (itemDeta.itemNames == null || itemDeta.itemNames.Count == 0)
        {
            Debug.Log("드랍할 아이템이 없습니다.");
            return;
        }

        // 랜덤으로 아이템 1개 선택 랜덤으로 0개부터 ,아이템 데이터의 아이템 네임 카운트가 최대값
        //랜덤 인덱스에 넣고, 드랍 아이템 네임에 뒤에 인덱스를 받아서 넣습니다.
        //드랍된 아이템 디버그에 적어서 호출합니다.
        int randomIndex = Random.Range(0, itemDeta.itemNames.Count);
        // 선택된 랜덤인덱스의 아이템 네임을 드랍 아이템 네임으로 넣은다음
        //디버그 로그에 드랍아이템 이름을 적는다.
        string droppedItemName = itemDeta.itemNames[randomIndex];

        Debug.Log("드랍된 아이템: " + droppedItemName);


        // 예: Instantiate(itemPrefab, transform.position, Quaternion.identity);
    }
}
