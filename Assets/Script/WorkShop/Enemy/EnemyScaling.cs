using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyScaling : MonoBehaviour
{
    [Header("Base stats เมื่อ LV 0")]
    public int baseHP = 50;
    public int baseATK = 10;
    public float baseMoveSpeed = 3f;
    public float baseAttackInterval = 1.2f;

    private Enemy enemy;
    private Character character;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        if (DifficultyManager.I != null)
        {
            ApplyDifficulty(DifficultyManager.I);
            DifficultyManager.I.OnLevelChanged += HandleLevelChanged;
        }
    }

    private void OnDisable()
    {
        if (DifficultyManager.I != null)
        {
            DifficultyManager.I.OnLevelChanged -= HandleLevelChanged;
        }
    }

    private void HandleLevelChanged(int lv)
    {
        ApplyDifficulty(DifficultyManager.I);
    }

    public void ApplyDifficulty(DifficultyManager dm)
    {
        if (dm == null || enemy == null || character == null) return;

        int newHP = Mathf.RoundToInt(baseHP * dm.HpMult());
        int newATK = Mathf.RoundToInt(baseATK * dm.AtkMult());
        float newMS = baseMoveSpeed * dm.SpeedMult();
        float newAtkInterval = baseAttackInterval * dm.AttackTimeMult();

        // เซ็ตกลับไปที่ Character / Enemy
        character.maxHealth = Mathf.Max(character.maxHealth, newHP);
        character.health = Mathf.Min(character.maxHealth, newHP);
        character.Damage = newATK;

        enemy.SetAttackInterval(newAtkInterval);
        character.movementSpeed = newMS;

        //  Debug ค่าใหม่ของศัตรูตัวนี้
        Debug.Log(
            $"<color=#FFA500>[Enemy Scaling]</color> {name} stats updated:" +
            $"\n  HP   : {newHP}" +
            $"\n  ATK  : {newATK}" +
            $"\n  Move : {newMS:F2}" +
            $"\n  AtkInterval : {newAtkInterval:F2}" +
            $"\n  Current Difficulty LV : {dm.Level}"
        );
    }
}
