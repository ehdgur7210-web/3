using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MoveStrategy
{
    public abstract void Move(Monster owner, Vector3 direction);
}
public class WalkMoveStrategy : MoveStrategy
{

    public override void Move(Monster owner, Vector3 direction)
    {
        Vector3 oldPos = owner.transform.position;

        // 이동
        owner.transform.position += direction * owner.MoveSpeed * Time.deltaTime;

        // 이동 후 위치
        Vector3 newPos = owner.transform.position;
       
    }

}
