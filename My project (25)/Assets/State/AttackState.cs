using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackState : IStateable
{
    private Monster owner;

    public AttackState(Monster owner)
    {
        this.owner = owner;
    }

   
    public void Enter()
    {
        Debug.Log("공격 시작!");

        // 공격 쿨타임
        owner.attackCooldownTimer = 0;

      
    }

    
    // 공격 실행
    
    public void Execute()
    {
        // 타겟이 없으면 대기 상태로
        if (owner.target == null)
        {
            owner.ChangeState(new IdleState(owner));
            return;
        }

        // 타겟까지의 거리
        float distanceToTarget = Vector3.Distance(
            owner.transform.position,
            owner.target.transform.position
        );

        // 공격 범위를 벗어나면 추적 상태로
        if (distanceToTarget > owner.attackRange)
        {
            owner.ChangeState(new ChaseState(owner));
            return;
        }

        // 타겟 바라보기
        //방향 얻는거
        Vector3 direction = owner.target.transform.position - owner.transform.position;
        direction.y = 0;
        //거리가 안에있다.
        if (direction != Vector3.zero)
        {
            //그 방향으로 고개를 돌려~
            owner.transform.rotation = Quaternion.LookRotation(direction);
        }

        // 공격 쿨타임 체크
        if (owner.attackCooldownTimer <= 0)
        {
            // 공격 전략 패턴 사용
            owner.PerformAttack();

            // 쿨타임 리셋
            owner.attackCooldownTimer = owner.attackCooldown;
        }
        else
        {
            // 쿨타임을 시간에따라 빼줌
            owner.attackCooldownTimer -= Time.deltaTime;
        }
    }

   
    public void Exit()
    {
        Debug.Log("아군이다");
    }
}
