using UnityEngine;
using TMPro;

public class GroundNote : MonoBehaviour
{
    public GameObject notePanel;
    public TextMeshProUGUI noteText;
    [TextArea(3, 10)]
    public string content;

    public GameObject pressEHint;

    bool _playerInRange = false;
    bool _isOpen = false;

    void Start()
    {
        if (notePanel != null) notePanel.SetActive(false);
        if (pressEHint != null) pressEHint.SetActive(false);
    }

    void Update()
    {
        if (!_playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleNote();
        }
    }

    void ToggleNote()
    {
        _isOpen = !_isOpen;
        notePanel.SetActive(_isOpen);
        pressEHint.SetActive(!_isOpen);

        if (_isOpen)
        {
            noteText.text = content;
            // Time.timeScale = 0f; // ถ้าต้องการหยุดเกม
        }
        else
        {
            // Time.timeScale = 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = true;
        pressEHint.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _playerInRange = false;
        pressEHint.SetActive(false);
        notePanel.SetActive(false);
        _isOpen = false;
    }
}

