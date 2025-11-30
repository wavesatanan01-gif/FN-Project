using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    [Header("Config")]
    public List<Skill> skillsSet = new List<Skill>();
    public GameObject[] skillEffects;

    // runtime
    private readonly List<Skill> durationSkills = new List<Skill>();
    private Player player;

    // unlock tracking
    public int relicCount = 0;
    public int killCount = 0;

    private void OnEnable()
    {
        Enemy.OnAnyEnemyKilled += OnEnemyKilled;
    }

    private void OnDisable()
    {
        Enemy.OnAnyEnemyKilled -= OnEnemyKilled;
    }

    private void Start()
    {
        player = GetComponent<Player>();

        var fireball = new FireballSkill
        {
            unlocked = false,
            unlockHint = "เก็บ Relic ชิ้นที่ 2 เพื่อปลด Fireball"
        };

        var heal = new HealSkill
        {
            unlocked = true,
            unlockHint = ""
        };

        var moveSpeed = new BuffSkillMoveSpeed
        {
            unlocked = false,
            unlockHint = "เก็บ Relic ชิ้นที่ 1 เพื่อปลด Speed Boost"
        };

        var atkBuff = new BuffSkillAttack
        {
            unlocked = false,
            unlockHint = "กำจัดมอนครบ 5 ตัว เพื่อปลด Attack Buff"
        };

        var defBuff = new BuffSkillDefense
        {
            unlocked = false,
            unlockHint = "กำจัดมอนครบ 5 ตัว เพื่อปลด BuffSkillDefense"
        };

        var stopMove = new DebuffSkillStopMove
        {
            unlocked = false,
            unlockHint = ""
        };

        skillsSet.Add(fireball);   // index 0
        skillsSet.Add(heal);       // index 1
        skillsSet.Add(moveSpeed);  // index 2
        skillsSet.Add(atkBuff);    // index 3
        skillsSet.Add(defBuff);    // index 4
        skillsSet.Add(stopMove);   // index 5
    }

    private void Update()
    {
        // ปุ่มเลข 1–6 เรียกใช้ skill
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseSkill(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) UseSkill(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) UseSkill(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) UseSkill(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5)) UseSkill(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6)) UseSkill(5);

        // อัปเดตสกิลแบบมีระยะเวลา
        float dt = Time.deltaTime;
        for (int i = durationSkills.Count - 1; i >= 0; i--)
        {
            Skill s = durationSkills[i];
            s.UpdateSkill(player);
            if (s.timer <= 0f)
            {
                durationSkills.RemoveAt(i);
            }
        }
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skillsSet.Count)
        {
            Debug.LogWarning($"Skill index {index} out of range. Total skills: {skillsSet.Count}");
            return;
        }

        Skill s = skillsSet[index];

        if (!s.unlocked)
        {
            string hint = string.IsNullOrEmpty(s.unlockHint) ? "ยังไม่ปลดล็อค" : s.unlockHint;
            Debug.Log($"Skill {s.skillName} ยังไม่ปลดล็อค: {hint}");
            return;
        }

        if (!s.IsReady(Time.time))
        {
            float remain = s.lastUsedTime + s.cooldownTime - Time.time;
            Debug.Log($"Skill {s.skillName} cooldown. Remain: {remain:F2}s");
            return;
        }

        if (skillEffects != null && index < skillEffects.Length && skillEffects[index] != null)
        {
            Vector3 spawnPos;
            Quaternion spawnRot = player.transform.rotation;

            if (index == 0)
            {
                spawnPos = player.transform.position + player.transform.forward * 1.0f;
                spawnPos = player.transform.position + Vector3.up * 0.5f;
            }
            else
            {
                spawnPos = player.transform.position;
            }
            if (index == 4)
            {
                spawnRot = Quaternion.Euler(90, player.transform.eulerAngles.y, 0);

            }
            else
            {
                spawnRot = player.transform.rotation;
            }

            GameObject fx = Instantiate(skillEffects[index], spawnPos, spawnRot, player.transform);
            Destroy(fx, 2.5f);
        }

        s.Activate(player);
        s.TimeStampSkill(Time.time);

        if (s.timer > 0f && !durationSkills.Contains(s))
        {
            durationSkills.Add(s);
        }
    }

    public void UnlockSkill(int index)
    {
        if (index < 0 || index >= skillsSet.Count) return;
        if (!skillsSet[index].unlocked)
        {
            skillsSet[index].unlocked = true;
            Debug.Log($"[SKILL UNLOCK] ปลดล็อค {skillsSet[index].skillName}");
        }
    }

    public void UnlockSkillByName(string name)
    {
        for (int i = 0; i < skillsSet.Count; i++)
        {
            if (skillsSet[i].skillName == name)
            {
                UnlockSkill(i);
                return;
            }
        }
        Debug.LogWarning($"[SKILL UNLOCK] ไม่พบสกิลชื่อ {name}");
    }

    private void OnEnemyKilled(Enemy e)
    {
        killCount++;
        Debug.Log($"[KILL] total = {killCount}");

        if (killCount == 5)
        {
            UnlockSkillByName("Attack Boost");
        }
        if (killCount == 10)
        {
            UnlockSkillByName("BuffSkillDefense");
        }
    }
}
