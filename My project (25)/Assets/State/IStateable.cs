using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateable
{
    void Enter();
    void Execute();

    void Exit();

}
