using System;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : Skill
{
    public float searchRadius = 5f;

    public int impactDamage = 10;

    public FireballSkill()
    {
        this.skillName = "Fireball";
        this.cooldownTime = 5f;
    }

    public override void Activate(Character character)
    {
        int atk = character.DamageEffective;
        Debug.Log($"[SKILL] {character.Name} cast Fireball (ATK = {atk})");

        Enemy[] targets = GetEnemysInRange(character);
        Debug.Log($"[SKILL] Fireball found {targets.Length} enemies in range {searchRadius}");

        if (targets.Length == 0)
        {
            return;
        }

        foreach (var enemy in targets)
        {
            if (enemy == null) continue;

            int finalImpactDamage = atk + impactDamage;
            enemy.TakeDamage(finalImpactDamage);

            Debug.Log($"[FIREBALL] Hit {enemy.Name} for {finalImpactDamage} impact damage (atk {atk} + base {impactDamage}).");

            DamageOverTimeEffect burn = new DamageOverTimeEffect
            {
                effectName = "Fireball Burn",
                duration = 5f,
                damagePerSecond = Mathf.Max(1f, atk * 0.2f),
                allowStack = false
            };
            enemy.ApplyEffect(burn);

            Debug.Log($"[FIREBALL] Apply burn to {enemy.Name}: {burn.damagePerSecond}/s for {burn.duration}s");
        }
    }


    public override void Deactivate(Character character)
    {
    }

    public override void UpdateSkill(Character character)
    {
    }

    private Enemy[] GetEnemysInRange(Character caster)
    {
        Collider[] hitColliders = Physics.OverlapSphere(caster.transform.position, searchRadius);
        List<Enemy> enemies = new List<Enemy>();

        foreach (var hitCollider in hitColliders)
        {
            Enemy targetCharacter = hitCollider.GetComponentInParent<Enemy>();
            if (targetCharacter != null && targetCharacter != caster && !enemies.Contains(targetCharacter))
            {
                enemies.Add(targetCharacter);
            }
        }

        return enemies.ToArray();
    }

}
