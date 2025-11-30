using UnityEngine;

public class Enemy : Character
{
    public static System.Action<Enemy> OnAnyEnemyKilled;

    [Header("Status VFX")]
    public GameObject burnStatusVfxPrefab;      
    [HideInInspector] public GameObject activeBurnVfxInstance;

    public GameObject poisonStatusVfxPrefab;
    [HideInInspector] public GameObject activePoisonVfxInstance;
    protected enum State { idel, cheses, attack, death }

    [SerializeField]
    protected float TimeToAttack = 1f;
    protected State currentState = State.idel;
    protected float timer = 0f;

    [Header("On-Hit Poison (สำหรับศัตรูประชิด)")]
    [Tooltip("เปิด/ปิด ฟังก์ชันตีแล้วติดพิษ")]
    public bool poisonOnHit = true;

    [Tooltip("ดาเมจพิษต่อวินาที")]
    public float poisonDamagePerSecond = 3f;

    [Tooltip("ระยะเวลาพิษ (วินาที)")]
    public float poisonDuration = 5f;

    public void SetAttackInterval(float v)
    {
        TimeToAttack = Mathf.Max(0.1f, v);
    }
 
    protected override void Update()
    {
        base.Update();
    }

    protected override void Turn(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }

    protected virtual void Attack(Player _player)
    {
        if (timer <= 0f)
        {
            _player.TakeDamage(Damage);

            if (animator != null)
                animator.SetBool("Attack", true);

            Debug.Log(Name + " attacks " + _player.Name + " for " + Damage + " damage.");

            if (poisonOnHit)
            {
                if (!_player.HasEffect("EnemyPoison"))
                {
                    DamageOverTimeEffect poisonDot = new DamageOverTimeEffect
                    {
                        effectName = "EnemyPoison",
                        duration = poisonDuration,
                        damagePerSecond = poisonDamagePerSecond
                    };

                    _player.ApplyEffect(poisonDot);
                    Debug.Log("[ENEMY POISON] Apply poison");
                }
                else
                {
                    Debug.Log("[ENEMY POISON] Already poisoned, skip apply");
                }
            }


            timer = TimeToAttack;
        }
    }
    public override void TakeDamage(int amount)
    {
        if (health <= 0) return;

        base.TakeDamage(amount);

        if (health <= 0)
        {
            OnAnyEnemyKilled?.Invoke(this);
        }
    }

    public override void TakePureDamage(int amount)
    {
        if (health <= 0) return;

        base.TakePureDamage(amount);

        if (health <= 0)
        {
            OnAnyEnemyKilled?.Invoke(this);
        }
    }
}



