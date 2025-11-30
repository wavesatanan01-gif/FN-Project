using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public static GameTimer I;

    [Header("Game Time Settings")]
    public float totalTime = 300f;     // เวลาเล่นทั้งหมด (วินาที) – ปรับได้ใน Inspector
    [HideInInspector] public float timeRemaining;

    [Header("Gas Settings")]
    public float gasLastSeconds = 10f; // ให้ควันเริ่มตอน 10 วินาทีสุดท้าย

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        // เริ่มเกมให้เวลาเหลือเท่ากับ totalTime
        timeRemaining = totalTime;
        Debug.Log($"[TIMER] Game start. totalTime = {totalTime} sec");
    }

    private void Update()
    {
        if (timeRemaining <= 0f) return;

        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0f) timeRemaining = 0f;

        // ถ้าเข้าช่วง 10 วินาทีสุดท้ายและยังไม่เปิดแก๊ส → ให้เรียก StartGas()
        if (!HazardManager.I.gasActive && timeRemaining <= gasLastSeconds)
        {
            Debug.Log($"[TIMER] Enter last {gasLastSeconds} seconds → start gas. timeRemaining = {timeRemaining:F2}");
            HazardManager.I.StartGas();
        }
    }
}
