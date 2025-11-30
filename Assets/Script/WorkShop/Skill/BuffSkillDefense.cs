using UnityEngine;

public class BuffSkillDefense : Skill
{
    public float duration = 5f;
    public float flatAdd = 5f;       
    public float percentAdd = 0.2f;  

    public BuffSkillDefense()
    {
        skillName = "Defense Boost";
        cooldownTime = 7f;
    }

    public override void Activate(Character character)
    {
        var effect = new StatBuffEffect();
        effect.effectName = "Defense Boost";
        effect.duration = duration;
        effect.targetStat = StatType.Defense;
        effect.flatAdd = flatAdd;
        effect.percentAdd = percentAdd;

        character.ApplyEffect(effect);

        Debug.Log("DefenseEffective now: " + character.DefenseEffective);
    }


    public override void UpdateSkill(Character character)
    {
    }

    public override void Deactivate(Character character)
    {
    }
}
