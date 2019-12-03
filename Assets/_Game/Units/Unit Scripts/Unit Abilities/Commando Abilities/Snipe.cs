using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Snipe is an Attack that deals damage in a long-range straight line
// it attacks the first Unit that is in the way of the attack; it does not go through that Unit

public class Snipe : Attack
{
    public override bool isAOE()
    {
        return false;
    }

    public override void DealEffects(Unit target, Unit source)
    {
        GameObject bullet = GameObject.Instantiate(Resources.Load("Prefabs/Bullet"), source.transform.position, Quaternion.Euler(new Vector3(0,0, (float)source.GetDirection()))) as GameObject;
        GameObject.Destroy(bullet, .5f);
        int bonusDamage = 0;
        if (source.attackBuffed)
            bonusDamage = 10;
        if (target != null)
        {
            target.ChangeHealth(((GetDamage() + bonusDamage) * (-1)), source, this);
        }
    }

    public override List<Vector2Int> GetAreaOfEffect(Vector2Int source, Direction direction)
    {
        return AttackHelper.GetLineAOE(source, direction, GetRange());
    }

    public override int GetDamage()
    {
        return 20;
    }

    public override List<EffectState> GetEffectState()
    {
        return new List<EffectState> { EffectState.DAMAGE };
    }

    public override string GetName()
    {
        return "Snipe";
    }

    public override int GetRange()
    {
        return 16;
    }

    protected override bool InferiorComparator(UnitAbility inQuestion)
    {
        return (inQuestion.GetType() == typeof(Snipe));
    }

    public override string GetSoundEvent()
    {
        return "event:/SHA/SHA1/SHA_Snipe";
    }
}
