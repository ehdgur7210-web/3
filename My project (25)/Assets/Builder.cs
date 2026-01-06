using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBuilder : MonoBehaviour
{
    private string monsterName = "";
    private int hp = 100;
    private float atk = 10f;
    private float def = 5f;
    private float moveSpeed = 3f;
    private float detectionRange = 10f;
    private float attackRange = 2f;
    private float attackCooldown = 2f;

    // 전략 패턴
    private AttackStrategy attackStrategy;
    private MoveStrategy moveStrategy;

    public MonsterBuilder SetName(string name)
    {
        this.monsterName = name;
        return this;
    }

    public MonsterBuilder SetHp(int hp)
    {
        this.hp = hp;
        return this;
    }

    public MonsterBuilder SetAtk(float atk)
    {
        this.atk = atk;
        return this;
    }

    public MonsterBuilder SetDef(float def)
    {
        this.def = def;
        return this;
    }

    public MonsterBuilder SetMoveSpeed(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        return this;
    }

    public MonsterBuilder SetDetectionRange(float range)
    {
        this.detectionRange = range;
        return this;
    }

    public MonsterBuilder SetAttackRange(float range)
    {
        this.attackRange = range;
        return this;
    }

    public MonsterBuilder SetAttackCooldown(float cooldown)
    {
        this.attackCooldown = cooldown;
        return this;
    }

    public MonsterBuilder SetAttackStrategy(AttackStrategy strategy)
    {
        this.attackStrategy = strategy;
        return this;
    }

 
    public MonsterBuilder SetMoveStrategy(MoveStrategy strategy)
    {
        this.moveStrategy = strategy;
        return this;
    }

  
    public void Build(Monster monster)
    {
        if (monster == null)
        {
            Debug.LogError("Monster가 null입니다!");
            return;
        }

        // 몬스터에 설정 적용
        monster.gameObject.name = monsterName;
        monster.Hp = hp;
        monster.Atk = atk;
        monster.Def = def;
        monster.MoveSpeed = moveSpeed;
        monster.detectionRange = detectionRange;
        monster.attackRange = attackRange;
        monster.attackCooldown = attackCooldown;

        // 몬스터에서 전략 함수설정을 만들어서 불러와서 실행을 한다.팩토리메서드
        if (attackStrategy != null)
        {
            monster.SetAttackStrategy(attackStrategy);
        }

        if (moveStrategy != null)
        {
            monster.SetMoveStrategy(moveStrategy);
        }

        Debug.Log($"몬스터 빌드 완료: {monsterName} (HP: {hp}, 공격력: {atk})");
    }
}
