using UnityEngine;

public class BuffSkillAttack : Skill
{
    public float duration = 5f;
    public float flatAdd = 5f;
    public float percentAdd = 0.2f;

    public float poisonDuration = 5f;
    public float poisonDamagePerSecond = 3f;

    public BuffSkillAttack()
    {
        skillName = "Attack Boost";
        cooldownTime = 7f;
    }

    public override void Activate(Character character)
    {
        float before = character.DamageEffective;

        // 1) ºÑ¾ ATK áººà´ÔÁ
        var atkBuff = new StatBuffEffect
        {
            effectName = "Attack Boost",
            duration = duration,
            targetStat = StatType.Attack,
            flatAdd = flatAdd,
            percentAdd = percentAdd
        };
        character.ApplyEffect(atkBuff);

        // 2) ºÑ¾ "â¨ÁµÕáÅéÇµÔ´¾ÔÉ"
        var poisonBuff = new OnHitPoisonEffect
        {
            effectName = "Attack Poison OnHit",
            duration = duration,                  
            poisonDuration = poisonDuration,     
            poisonDamagePerSecond = poisonDamagePerSecond
        };
        character.ApplyEffect(poisonBuff);

        float after = character.DamageEffective;
        Debug.Log($"[BUFF] {character.Name} ATK: {before} -> {after} (for {duration}s)");
        Debug.Log($"[BUFF] {character.Name} attacks now inflict poison {poisonDamagePerSecond}/s for {poisonDuration}s");
    }

    public override void UpdateSkill(Character character)
    {
    }

    public override void Deactivate(Character character)
    {
    }
}
