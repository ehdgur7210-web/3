using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class ChaseState : IStateable
{
    public Monster Owner;

    public ChaseState(Monster owner)
        { this.Owner = owner; }
    public void Enter()
    {
        Debug.Log("추적 시작");
    }

    public void Execute()
    {
        if (Owner.target == null)
        {
            Owner.ChangeState(new IdleState(Owner));
            return;
        }

        // 타겟 방향 계산
        Vector3 direction = Owner.target.transform.position - Owner.transform.position;
        direction.y = 0; 

        float distanceToTarget = direction.magnitude;

        // 공격 범위 안에 들어오면 공격 상태로
        if (distanceToTarget <= Owner.attackRange)
        {
            Owner.ChangeState(new AttackState(Owner));
            return;
        }

        // 감지 범위를 벗어나면 대기 상태로
        if (distanceToTarget > Owner.detectionRange)
        {
            Owner.ChangeState(new IdleState(Owner));
            return;
        }

        // 이동 전략 패턴 사용
        if (Owner.moveStrategy != null)
        {
            Owner.moveStrategy.Move(Owner, direction.normalized);
        }
        else
        {
            // 기본 이동 (전략 없을 때)
            Owner.transform.position += direction.normalized * Owner.MoveSpeed * Time.deltaTime;
        }

        // 타겟을 바라보도록 회전
        if (direction != Vector3.zero)
        {
            Owner.transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    public void Exit()
    {
        Debug.Log("추적종료");
    }

}
