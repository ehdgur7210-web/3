using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;


public class Slime : Monster
{
    protected override void Start()
    {
       //빌더를 가져와서 빌더를 추가하고.
        MonsterBuilder builder = new MonsterBuilder();

        builder
       .SetName("슬라임")
       .SetHp(50)
       .SetAtk(5)
       .SetDef(2)
       .SetMoveSpeed(1.5f)
       .SetDetectionRange(8f)
       .SetAttackRange(1.5f)
       .SetAttackCooldown(1.5f)
       .SetAttackStrategy(new MeleeAttackStrategy())  //전략 정할수있다.
       .SetMoveStrategy(new WalkMoveStrategy())     //이동도 전략패턴으로 사용할수 있다.
       .Build(this);  // 이 슬라임에 적용

        // 부모 클래스 초기화
        base.Start();
    }
}
