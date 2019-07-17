using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : UnitAbility
{
    public override void Execute(Unit source)
    {
        throw new System.NotImplementedException();
    }

    public override int GetRange()
    {
        return 1;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(BasicAttack));
    }
}
