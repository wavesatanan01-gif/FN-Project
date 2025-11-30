using UnityEngine;

public class PortalGateOpener : MonoBehaviour
{
    [Header("Portal Objects")]
    [SerializeField] private GameObject closedPortal;
    [SerializeField] private GameObject openedPortal;

    [Header("เงื่อนไขเก็บของ")]
    [SerializeField] private int relicNeeded = 3;

    private int relicCount = 0;
    private bool isOpened = false;
    public bool IsOpened => isOpened;


    private void Start()
    {
        if (closedPortal != null)
            closedPortal.SetActive(true);

        if (openedPortal != null)
            openedPortal.SetActive(false);
    }

    public void OnRelicCollected()
    {
        if (isOpened) return;

        relicCount++;
        Debug.Log($"[PORTAL] Relic collected {relicCount}/{relicNeeded}");

        if (relicCount >= relicNeeded)
        {
            OpenPortal();
        }
    }

    private void OpenPortal()
    {
        if (isOpened) return;
        isOpened = true;

        closedPortal.SetActive(false);
        openedPortal.SetActive(true);

        Debug.Log("[PORTAL] Portal is now open. Please enter to win.");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isOpened) return; // ยังไม่เปิดห้ามชนะ

        if (other.CompareTag("Player"))
        {
            Debug.Log("[WIN] Player entered the portal! You WIN!");

            Time.timeScale = 0f; // หยุดเกม

            // เรียกขึ้น UI ชนะ
            GameManager.instance.ShowWin();
        }
    }
}
