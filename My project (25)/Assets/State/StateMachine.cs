using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//자식에있는 것들 인터페이스로 뺴서 진입 실행 나가기로 나눠서 
//기존에 있던 FSM방식에서 상태머신을 대기상태, 추적상태, 공격상태로 나눴습니다.


//상태머신
public class StateMachine
{
    //현재 상태를 담을 것
    private IStateable currentState;


    // 현재 상태를 인터페이스 변수값을 메개변수로 받아서 상태를 적용하고 초기 상태는 현재상태의 Enter
    public void Initialize(IStateable startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

   //현재 상태가 있으면 그전에 대기상태의 Exit를 호출해서 나가고 , 새로운 상태로 변경 하는거
    public void ChangeState(IStateable newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    
    //현재 상태를 Enter 에서 Execute로 업데이트 시켜줍니다.
  
    public void Update()
    {
        if (currentState != null)
        {
            currentState.Execute();
        }
    }

    //현재 상태를 반환해줍니다. 변수선언이 private라
    
    public IStateable GetCurrentState()
    {
        return currentState;
    }
}