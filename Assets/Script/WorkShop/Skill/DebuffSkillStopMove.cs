using UnityEngine;

public class DebuffSkillStopMove : Skill
{
    public float searchRadius = 5f;
    public float freezeDuration = 3f;

    public DebuffSkillStopMove()
    {
        skillName = "Stop Move";
        cooldownTime = 6f;
    }

    public override void Activate(Character character)
    {
        Debug.Log(character.Name + " uses Stop Move!");

        Enemy[] targets = GetEnemysInRange(character);
        if (targets.Length == 0)
        {
            Debug.Log("No enemies in range for Stop Move.");
            return;
        }

        foreach (Enemy enemy in targets)
        {
            if (enemy == null) continue;

            StatBuffEffect stopMove = new StatBuffEffect
            {
                effectName = "Stop Move",
                duration = freezeDuration,
                targetStat = StatType.MoveSpeed,
                flatAdd = 0f,
                percentAdd = -1f
            };

            enemy.ApplyEffect(stopMove);

            Debug.Log("Stop Move applied to " + enemy.Name +
                      " for " + freezeDuration + " seconds.");
        }
    }

    public override void UpdateSkill(Character character)
    {
    }

    public override void Deactivate(Character character)
    {
    }

    private Enemy[] GetEnemysInRange(Character character)
    {
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();
        System.Collections.Generic.List<Enemy> result = new System.Collections.Generic.List<Enemy>();

        Vector3 origin = character.transform.position;

        foreach (Enemy e in allEnemies)
        {
            if (e == null) continue;

            float dist = Vector3.Distance(origin, e.transform.position);
            if (dist <= searchRadius)
            {
                result.Add(e);
            }
        }

        return result.ToArray();
    }
}
