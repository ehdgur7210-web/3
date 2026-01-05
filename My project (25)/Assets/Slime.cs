using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Slime : Monster
{
    //부모의 전략을 가져와서 

    protected override void Start()
    {

        Hp = 50;
        Atk = 5;
        Def = 2;
        MoveSpeed = 1.5f;


        detectionRange = 8f;
        attackRange = 1.5f;
        attackCooldown = 1.5f;

        //  공격 전략 설정 

        AS = new MeleeAttackStrategy();

        // 부모 클래스의 Start 호출 
        // 상태머신, 아이템 데이터, 타겟 찾기 등
        base.Start();

        Debug.Log("슬라임 초기화 완료! HP: " + Hp + ", 공격력: " + Atk);
    }


}
