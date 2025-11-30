using UnityEngine;

using UnityEngine;

public class EnemyRange : Enemy
{
    [Header("ระยะโจมตีของศัตรูไกล")]
    public float attackRange = 5f;

    [Header("On-Hit Debuff (สำหรับศัตรูตีไกล)")]
    [Tooltip("เปิด/ปิด ฟังก์ชันตีแล้วติดดีบัฟลดสเตตัส")]
    public bool debuffOnHit = true;

    [Tooltip("ระยะเวลาที่ดีบัฟมีผล (วินาที)")]
    public float debuffDuration = 4f;

    [Tooltip("เปอร์เซ็นต์ลดความเร็ว เช่น -0.5 = ลด 50%")]
    public float moveSpeedPercent = -0.5f;

    [Tooltip("เปอร์เซ็นต์ลดพลังโจมตี เช่น -0.3 = ลด 30% (0 = ไม่ลด)")]
    public float attackPercent = 0f;

    [Tooltip("เปอร์เซ็นต์ลดเกราะ เช่น -0.3 = ลด 30% (0 = ไม่ลด)")]
    public float defensePercent = 0f;

    protected override void Update()
    {
        base.Update();

        if (player == null)
        {
            if (animator != null)
                animator.SetBool("Attack", false);
            return;
        }

        Turn(player.transform.position - transform.position);
        timer -= Time.deltaTime;

        if (GetDistanPlayer() < attackRange)
        {
            Attack(player);
        }
        else
        {
            if (animator != null)
                animator.SetBool("Attack", false);
        }
    }

    protected override void Attack(Player _player)
    {
        if (timer <= 0f)
        {
            _player.TakeDamage(Damage);
            if (animator != null)
                animator.SetBool("Attack", true);

            Debug.Log(Name + " (Ranged) attacks " + _player.Name + " for " + Damage + " damage.");

            if (debuffOnHit && debuffDuration > 0f)
            {
                if (Mathf.Abs(moveSpeedPercent) > 0.0001f)
                {
                    StatBuffEffect slow = new StatBuffEffect
                    {
                        effectName = "Ranged_Slow",
                        duration = debuffDuration,
                        targetStat = StatType.MoveSpeed,
                        flatAdd = 0f,
                        percentAdd = moveSpeedPercent
                    };
                    _player.ApplyEffect(slow);
                }

                if (Mathf.Abs(attackPercent) > 0.0001f)
                {
                    StatBuffEffect atkDown = new StatBuffEffect
                    {
                        effectName = "Ranged_AtkDown",
                        duration = debuffDuration,
                        targetStat = StatType.Attack,
                        flatAdd = 0f,
                        percentAdd = attackPercent
                    };
                    _player.ApplyEffect(atkDown);
                }

                if (Mathf.Abs(defensePercent) > 0.0001f)
                {
                    StatBuffEffect defDown = new StatBuffEffect
                    {
                        effectName = "Ranged_DefDown",
                        duration = debuffDuration,
                        targetStat = StatType.Defense,
                        flatAdd = 0f,
                        percentAdd = defensePercent
                    };
                    _player.ApplyEffect(defDown);
                }

                Debug.Log("[RANGED DEBUFF] " + Name + " applied debuff(s) to " + _player.Name);
            }

            timer = TimeToAttack;
        }
    }
}

