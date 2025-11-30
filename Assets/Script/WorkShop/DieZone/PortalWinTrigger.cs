using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PortalWinTrigger : MonoBehaviour
{
    private PortalGateOpener opener;

    private void Awake()
    {
        opener = GetComponent<PortalGateOpener>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (opener != null && !opener.IsOpened)
        {
            Debug.Log("[PORTAL] Player touched portal, but it's not opened yet.");
            return;
        }

        Player player = other.GetComponentInParent<Player>();
        if (player == null) return;

        Debug.Log("<color=#00ff00>คุณชนะ! ผ่าน Portal สำเร็จ!</color>");
        Time.timeScale = 0f;
    }
}
