using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : UnitAbility
{
    public override void execute(Unit source)
    {
        source.hasAttacked = true;
        return;
    }

    public override int getRange()
    {
        return 0;
    }
}
