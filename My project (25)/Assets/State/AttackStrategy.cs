
using UnityEngine;

public abstract class AttackStrategy
{
    public abstract void Attack();
}

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

