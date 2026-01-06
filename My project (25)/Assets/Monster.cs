using Unity.VisualScripting;
using UnityEngine;


public class Monster : MonoBehaviour
{
    [Header("타겟")]
    public GameObject target;

    [Header("전략 패턴")]
    public AttackStrategy attackStrategy;
    public MoveStrategy moveStrategy;

    [Header("아이템")]
    private ItemDeta itemDeta;

    // 스탯 (빌더로 설정했다)
    private int hp;
    private float atk;
    private float def;
    private float moveSpeed;

    public bool isDead = false;
    private IStateable currentState;

    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 2f;
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
    public void ChangeState(IStateable newState)
    {
        Debug.Log($"[상태 전환] {gameObject.name}: {currentState?.GetType().Name} → {newState?.GetType().Name}");

        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }
    public IStateable GetCurrentState()
    {
        return currentState;
    }

    public void SetAttackStrategy(AttackStrategy strategy)
    {
        this.attackStrategy = strategy;
        Debug.Log($"{gameObject.name}: 공격 전략 설정 → {strategy?.GetType().Name}");
    }

    public void SetMoveStrategy(MoveStrategy strategy)
    {
        this.moveStrategy = strategy;
        Debug.Log($"{gameObject.name}: 이동 전략 설정 → {strategy?.GetType().Name}");
    }

    public void PerformAttack()
    {
        if (attackStrategy != null)
        {
            attackStrategy.Attack();
        }
        else
        {
            Debug.LogWarning($"{gameObject.name}: 공격 전략이 없습니다!");
        }
    }

    public void TakeDamage(float damage)
    {
        float damageTaken = damage - Def;
        if (damageTaken < 0) damageTaken = 0;

        hp -= (int)damageTaken;
        Debug.Log($"{gameObject.name}이(가) {damageTaken} 피해를 입음. 남은 HP: {hp}");

        Die();
    }

    public void Die()
    {
        if (hp <= 0)
        {
            hp = 0;
        }

        if (hp == 0 && !isDead)
        {
            isDead = true;
            Debug.Log($"{gameObject.name} 사망!");
            DropRandomItem();
            Destroy(gameObject, 2f);
        }
    }

    protected virtual void Start()
    {
        currentState = new IdleState(this);
        currentState.Enter();
        Debug.Log($"[Monster.Start] {gameObject.name}: 초기 상태 설정 완료 → IdleState");

        // 아이템 초기화
        itemDeta = gameObject.AddComponent<ItemDeta>();
        itemDeta.DropItem();

        // 기본 공격 전략 설정 (없으면)
        if (attackStrategy == null)
        {
            attackStrategy = new MeleeAttackStrategy();
        }

        // 타겟 찾기
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player;
        }

        Debug.Log($"{gameObject.name} 몬스터 생성 - HP: {hp}");
    }

    protected virtual void Update()
    {
        if (isDead) return;

        if (currentState == null)
        {
            Debug.LogError($"{gameObject.name}: currentState가 null입니다!");
            return;
        }
        currentState.Execute();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(50);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformAttack();
        }
    }

    
    //랜덤 아이템 드랍

    private void DropRandomItem()
    {
        if (itemDeta.itemNames == null || itemDeta.itemNames.Count == 0)
        {
            Debug.Log("드랍할 아이템이 없습니다.");
            return;
        }

        int randomIndex = Random.Range(0, itemDeta.itemNames.Count);
        string droppedItemName = itemDeta.itemNames[randomIndex];

        Debug.Log($"드랍된 아이템: {droppedItemName}");
    }
}