using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : Identity, Idestoryable
{
    int _health;
    public int health
    {
        get { return _health; }
        set { _health = Mathf.Clamp(value, 0, _maxHealth); }
    }

    public event System.Action<StatusEffect> OnEffectApplied;
    public event System.Action<StatusEffect> OnEffectRemoved;

    public int maxHealth { get => _maxHealth; set => _maxHealth = value; }

    [SerializeField]
    private int _maxHealth = 100;

    [Header("Base Stats")]
    [SerializeField] private int baseDamage = 10;
    [SerializeField] private int baseDefense = 10;
    [SerializeField] private float baseMoveSpeed = 5f;
    public float movementSpeed
    {
        get => baseMoveSpeed;
        set => baseMoveSpeed = value;
    }

    public int BaseDamage { get => baseDamage; set => baseDamage = value; }
    public int BaseDefense { get => baseDefense; set => baseDefense = value; }

    public int Damage { get => baseDamage; set => baseDamage = value; }
    public int Deffent { get => baseDefense; set => baseDefense = value; }

    public List<StatusEffect> ActiveEffects = new List<StatusEffect>();
    public bool HasEffect(string effectName)
    {
        foreach (var e in ActiveEffects)
        {
            if (e.effectName == effectName)
                return true;
        }
        return false;
    }

    public int DamageEffective
    {
        get
        {
            float flat = 0f, percent = 0f;
            foreach (var e in ActiveEffects.OfType<StatBuffEffect>())
            {
                if (e.targetStat == StatType.Attack)
                {
                    flat += e.flatAdd;
                    percent += e.percentAdd;
                }
            }
            return Mathf.RoundToInt((baseDamage + flat) * (1f + percent));
        }
    }

    public int DefenseEffective
    {
        get
        {
            float flat = 0f, percent = 0f;
            foreach (var e in ActiveEffects.OfType<StatBuffEffect>())
            {
                if (e.targetStat == StatType.Defense)
                {
                    flat += e.flatAdd;
                    percent += e.percentAdd;
                }
            }
            return Mathf.RoundToInt((baseDefense + flat) * (1f + percent));
        }
    }

    public float MoveSpeedEffective
    {
        get
        {
            float flat = 0f, percent = 0f;
            foreach (var e in ActiveEffects.OfType<StatBuffEffect>())
            {
                if (e.targetStat == StatType.MoveSpeed)
                {
                    flat += e.flatAdd;
                    percent += e.percentAdd;
                }
            }
            return (baseMoveSpeed + flat) * (1f + percent);
        }
    }

    protected Animator animator;
    protected Rigidbody rb;
    Quaternion newRotation;

    public event Action<Idestoryable> OnDestory;

    public override void SetUP()
    {
        base.SetUP();
        health = maxHealth;
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"Animator not found on {gameObject.name}, but continuing.");
        }

    }

    public void ApplyEffect(StatusEffect effect)
    {
        if (effect == null) return;

        if (!effect.allowStack)
        {
            for (int i = 0; i < ActiveEffects.Count; i++)
            {
                StatusEffect existing = ActiveEffects[i];

                if (existing.effectName == effect.effectName &&
                    existing.GetType() == effect.GetType())
                {
                    existing.duration = effect.duration;
                    existing.elapsed = 0f;

                    Debug.Log("[EFFECT REFRESH] " + effect.effectName + " on " + Name);

                    OnEffectApplied?.Invoke(existing);
                    return;
                }
            }
        }

        effect.elapsed = 0f;
        ActiveEffects.Add(effect);
        effect.OnApply(this);

        Debug.Log("ApplyEffect: " + effect.effectName + " to " + Name);

        OnEffectApplied?.Invoke(effect);
    }

    public void TickStatusEffects()
    {
        float dt = Time.deltaTime;

        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect effect = ActiveEffects[i];
            if (effect == null)
            {
                ActiveEffects.RemoveAt(i);
                continue;
            }

            effect.Tick(this, dt);

            if (effect.IsFinished)
            {
                RemoveEffect(effect);
            }
        }
    }

    public void RemoveEffect(StatusEffect effect)
    {
        if (effect == null) return;

        effect.OnRemove(this);
        ActiveEffects.Remove(effect);

        Debug.Log("[EFFECT END] " + effect.effectName + " removed from " + Name +
                  ". Current MoveSpeedEffective = " + MoveSpeedEffective);
        Debug.Log("Defense Boost ended. DefenseEffective now: " + DefenseEffective);

        OnEffectRemoved?.Invoke(effect);

    }


    protected virtual void Update()
    {
        float dt = Time.deltaTime;

        if (ActiveEffects.Count > 0)
        {
            Debug.Log("[Effect Tick] " + Name + " has " + ActiveEffects.Count + " active effects.");
        }

        for (int i = ActiveEffects.Count - 1; i >= 0; i--)
        {
            StatusEffect effect = ActiveEffects[i];

            Debug.Log("[Effect Tick] " + Name + " ticking " +
                      effect.effectName + " (" + effect.GetType().Name + ")");

            effect.Tick(this, dt);

            if (effect.IsFinished)
            {
                RemoveEffect(effect);
            }
        }
    }


    public virtual void TakeDamage(int amount)
    {
        int before = health;

        amount = Mathf.Clamp(amount - DefenseEffective, 1, amount);
        health -= amount;

        if (this is Enemy)
        {
            Debug.Log(
                "[ENEMY HP] " + Name +
                " : " + before + " -> " + health +
                " (damage " + amount + ")"
            );
        }

        if (health <= 0)
        {
            OnDestory?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public virtual void TakePureDamage(int amount)
    {
        int before = health;

        health = Mathf.Clamp(health - amount, 0, maxHealth);

        Debug.Log($"[PURE DAMAGE] {Name} : {before} -> {health} (damage {amount})");

        if (health <= 0)
        {
            OnDestory?.Invoke(this);
            Destroy(gameObject);
        }
    }


    public virtual void Heal(int amount)
    {
        int before = health;

        health = Mathf.Clamp(health + amount, 0, maxHealth);

        Debug.Log("Heal() called. Before: " + before + " After: " + health + " Amount: +" + amount);
    }


    protected virtual void Turn(Vector3 direction)
    {
        // ใช้กับศัตรู/ตัวอื่นได้ปกติ ถ้า direction == Vector3.zero ไม่ต้องหมุน
        if (direction == Vector3.zero) return;

        newRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 15f);
    }

    protected virtual void Move(Vector3 direction)
    {
        // ใช้ direction ตรงๆ แล้วคูณด้วยความเร็ว
        Vector3 velocity = direction * MoveSpeedEffective;
        velocity.y = rb.linearVelocity.y; // รักษาแกน Y จากฟิสิกส์ไว้ (เช่น แรงโน้มถ่วง)
        rb.linearVelocity = velocity;

        // ส่งค่า speed ให้ Animator (ไม่สนใจความเร็วแนวตั้ง)
        Vector2 horizontal = new Vector2(velocity.x, velocity.z);
        animator.SetFloat("Speed", horizontal.magnitude);
    }
}
