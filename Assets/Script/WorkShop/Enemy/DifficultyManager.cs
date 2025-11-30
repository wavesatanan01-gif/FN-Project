using UnityEngine;
using System;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager I { get; private set; }

    [Header("Scaling per Level (ต่อชิ้นที่เก็บ)")]
    [SerializeField] float hpPerLevel = 0.25f;  // +25% ต่อเลเวล
    [SerializeField] float atkPerLevel = 0.20f;  // +20%
    [SerializeField] float spdPerLevel = 0.10f;  // +10%
    [SerializeField] float atkRatePerLv = -0.10f; // -10% attack interval (ตีถี่ขึ้น)
    [SerializeField] float spawnPerLevel = 0.15f; // ใช้กับ Spawner ได้

    public int Level { get; private set; } = 0;
    public event Action<int> OnLevelChanged;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LevelUp()
    {
        Level++;

        // ✅ Debug ตอนมอนอัปเลเวล
        Debug.Log(
            $"<color=#00FF00>[DIFFICULTY]</color> Enemy difficulty increased → <b>LV {Level}</b>"
        );

        OnLevelChanged?.Invoke(Level);
    }

    // Multipliers
    public float HpMult() => 1f + hpPerLevel * Level;
    public float AtkMult() => 1f + atkPerLevel * Level;
    public float SpeedMult() => 1f + spdPerLevel * Level;
    public float AttackTimeMult() => Mathf.Max(0.2f, 1f + atkRatePerLv * Level);
    public float SpawnMult() => 1f + spawnPerLevel * Level;
}
