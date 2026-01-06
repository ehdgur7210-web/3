using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dragon : Monster
{
    protected override void Start()
    {
       
        MonsterBuilder builder = new MonsterBuilder();

        builder
            .SetName("드래곤")
            .SetHp(200)
            .SetAtk(20)
            .SetDef(10)
            .SetMoveSpeed(3f)
            .SetDetectionRange(15f)
            .SetAttackRange(5f)
            .SetAttackCooldown(3f)
            .SetAttackStrategy(new RangedAttackStrategy())
            .SetMoveStrategy(new WalkMoveStrategy()) 
            .Build(this);  // 이 드래곤에 적용

        // 부모 클래스 초기화
        base.Start();
    }
}