using UnityEngine;

public class DamageOverTimeEffect : StatusEffect
{
    public float damagePerSecond = 5f;
    private float damageAccumulator = 0f;

    private const string FireEffectName = "Fireball Burn";
    private const string PoisonEffectName = "Poison"; 

    public override void OnApply(Character c)
    {
        base.OnApply(c);

        Enemy enemy = c as Enemy;
        if (enemy == null) return;

        if (effectName == FireEffectName)
        {
            if (enemy.burnStatusVfxPrefab != null &&
                enemy.activeBurnVfxInstance == null)
            {
                enemy.activeBurnVfxInstance = GameObject.Instantiate(
                    enemy.burnStatusVfxPrefab,
                    enemy.transform
                );
            }
        }
        else if (effectName == PoisonEffectName)
        {
            if (enemy.poisonStatusVfxPrefab != null &&
                enemy.activePoisonVfxInstance == null)
            {
                enemy.activePoisonVfxInstance = GameObject.Instantiate(
                    enemy.poisonStatusVfxPrefab,
                    enemy.transform
                );
            }
        }
    }

    public override void OnRemove(Character c)
    {
        Enemy enemy = c as Enemy;

        if (enemy != null)
        {
            if (effectName == FireEffectName && enemy.activeBurnVfxInstance != null)
            {
                GameObject.Destroy(enemy.activeBurnVfxInstance);
                enemy.activeBurnVfxInstance = null;
            }
            else if (effectName == PoisonEffectName && enemy.activePoisonVfxInstance != null)
            {
                GameObject.Destroy(enemy.activePoisonVfxInstance);
                enemy.activePoisonVfxInstance = null;
            }
        }

        base.OnRemove(c);
    }

    public override void Tick(Character c, float dt)
    {
        base.Tick(c, dt);

        damageAccumulator += damagePerSecond * dt;
        int dmg = Mathf.FloorToInt(damageAccumulator);

        if (dmg > 0)
        {
            damageAccumulator -= dmg;
            c.TakeDamage(dmg);
        }
    }
}
