using TMPro;
using UnityEngine;
using UnityEngine.UI;

// กำหนดให้เป็น sealed เพื่อป้องกันการสืบทอด
public class GameManager : MonoBehaviour
{
    // 1. Private Static Field (The Singleton Instance)
    // ใช้ backing field เพื่อควบคุมการเข้าถึง
    public static GameManager instance;

    // 2. Public Static Property (Global Access Point)
   
    [Header("Game State")]
    public int currentScore = 0;
    public bool isGamePaused = false;

    [Header("UI Game")]
    public GameObject pauseMenuUI;
    public TMP_Text scoreText;
    public Slider HPBar;

    public GameObject gameOverPanel;
    public GameObject winPanel;
    public bool isGameOver = false;
    public bool isGameEnd = false;



    public void ShowWin()
    {
        Debug.Log(">>> ShowWin CALLED <<<");

        if (isGameEnd) return;          // กันเรียกซ้ำ
        isGameEnd = true;

        Time.timeScale = 0f;            // หยุดเวลาในเกม

        if (winPanel != null)
        {
            winPanel.SetActive(true);   // เปิด YOU ARE SURVIVOR
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ShowGameOver()
    {
        Debug.Log(">>> ShowGameOver CALLED <<<");

        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 3. Private Constructor Logic (ใช้ Awake() แทน Constructor ปกติใน Unity)
    private void Awake()
    {
        // ตรวจสอบว่ามี Instance อยู่แล้วหรือไม่
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
       
    }

    // ------------------- Singleton Functionality -------------------

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
       HPBar.value = currentHealth;
       HPBar.maxValue = maxHealth;
    }
    public void AddScore(int amount)
    {
        currentScore += amount;
        scoreText.text = currentScore.ToString();
    }

    public void TogglePause()
    {
       isGamePaused = !isGamePaused;
       Time.timeScale = isGamePaused ? 0 : 1;
       pauseMenuUI.SetActive(isGamePaused);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}