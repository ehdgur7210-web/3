using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class IdleState : IStateable
{
    private Monster owner;

    //생성자로 초기화 해서 몬스터를 받은다음
    public IdleState(Monster owner)
    {
        this.owner = owner;
    }
       
    public void Enter()
    {
        Debug.Log(owner.gameObject.name + "대기상태 :두리번 두리번");
    }

   

    public void Execute()
    {
        //타겟이 없으면 리턴
        if (owner.target == null) return;
        //타겟 까지의 거리계산
        float distanceToTarget = Vector3.Distance(owner.transform.position, owner.target.transform.position);
        //감지 범위 안에 들어오면 추격
        if (distanceToTarget <= owner.detectionRange)
        {
            owner.ChangeState(new ChaseState(owner));
        }
    }


    public void Exit()
    {
        Debug.Log("대기상태 끝");
    }
}
