using UnityEngine;

public class BuffSkillMoveSpeed : Skill
{
    public float duration = 3f;      
    public float percent = 0.5f;     

    public BuffSkillMoveSpeed()
    {
        skillName = "Speed Boost";
        cooldownTime = 5f;
    }

    public override void Activate(Character character)
    {
        float before = character.MoveSpeedEffective;

        var effect = new StatBuffEffect
        {
            effectName = "Speed Boost",
            duration = duration,
            targetStat = StatType.MoveSpeed,
            flatAdd = 0f,
            percentAdd = percent
        };

        character.ApplyEffect(effect);

        float after = character.MoveSpeedEffective;
        Debug.Log($"[BUFF] {character.Name} Speed: {before} -> {after} (for {duration}s)");
    }

    public override void UpdateSkill(Character character)
    {
        
    }

    public override void Deactivate(Character character)
    {
        
    }
}
