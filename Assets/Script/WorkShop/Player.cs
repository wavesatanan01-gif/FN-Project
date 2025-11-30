using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [Header("Hand setting")]
    public Transform RightHand;
    public Transform LeftHand;
    public List<Item> inventory = new List<Item>();

    Vector3 _inputDirection;
    bool _isAttacking = false;
    bool _isInteract = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        health = maxHealth;

        if (animator != null)
        {
            animator.applyRootMotion = false;
        }

        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX
                           | RigidbodyConstraints.FreezeRotationY
                           | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    public void FixedUpdate()
    {
        Move(_inputDirection);
        Attack(_isAttacking);
    }

    public void Update()
    {
        base.Update();
        HandleInput();
    }

    void LateUpdate()
    {
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }

    public void AddItem(Item item)
    {
        inventory.Add(item);
    }

    private void HandleInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward; forward.y = 0f; forward.Normalize();
        Vector3 right = transform.right; right.y = 0f; right.Normalize();

        _inputDirection = forward * y + right * x;

        if (_inputDirection.sqrMagnitude > 1f)
            _inputDirection.Normalize();

        if (Input.GetMouseButtonDown(0)) _isAttacking = true;
        if (Input.GetKeyDown(KeyCode.E)) _isInteract = true;
    }

    public void Attack(bool isAttacking)
    {
        if (isAttacking)
        {
            animator.SetTrigger("Attack");

            var target = InFront as Idestoryable;
            if (target != null)
            {
                int dmg = DamageEffective;
                target.TakeDamage(dmg);

                Enemy enemy = InFront as Enemy;
                if (enemy != null)
                {
                    OnHitPoisonEffect poisonBuff = null;

                    foreach (var eff in ActiveEffects)
                    {
                        poisonBuff = eff as OnHitPoisonEffect;
                        if (poisonBuff != null) break;
                    }

                    if (poisonBuff != null)
                    {
                        DamageOverTimeEffect poisonDot = new DamageOverTimeEffect
                        {
                            effectName = "Poison",
                            duration = poisonBuff.poisonDuration,
                            damagePerSecond = poisonBuff.poisonDamagePerSecond
                        };
                        enemy.ApplyEffect(poisonDot);
                    }
                }
            }
            _isAttacking = false;
        }
    }

    private void OnPlayerDied()
    {
        Debug.Log(">>> OnPlayerDied CALLED <<<");

        Time.timeScale = 0f;

        if (GameManager.instance != null)
        {
            GameManager.instance.ShowGameOver();
        }
    }


    public override void TakeDamage(int amount)
    {
        // ถ้าตายแล้ว ไม่ต้องโดนดาเมจซ้ำ
        if (health <= 0) return;

        // ลดเลือดเอง แทนการให้ base ทำ (กันเคสที่ base ทำลายตัวละคร)
        health -= amount;
        if (health < 0) health = 0;

        GameManager.instance.UpdateHealthBar(health, maxHealth);

        if (health <= 0)
        {
            OnPlayerDied();
        }
    }

    public override void TakePureDamage(int amount)
    {
        if (health <= 0) return;

        health -= amount;
        if (health < 0) health = 0;

        GameManager.instance.UpdateHealthBar(health, maxHealth);

        if (health <= 0)
        {
            OnPlayerDied();
        }
    }


}
