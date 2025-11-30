using UnityEngine;
using TMPro;

public class GameTimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    float time;

    void Update()
    {
        // ถ้าเกมโอเวอร์แล้ว ไม่ต้องนับต่อ
        if (GameManager.instance != null && GameManager.instance.isGameOver)
            return;

        time += Time.deltaTime;   // เคารพ Time.timeScale

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}