using UnityEngine;

public class SkillUnlockPickup : MonoBehaviour
{
    [Header("ปลดล็อกจากชื่อ (optional)")]
    public string skillToUnlock = "";

    [Header("หรือปลดล็อกจากช่อง index")]
    public int skillIndex = -1;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Player player = other.GetComponent<Player>();
        if (player == null) return;

        SkillBook book = player.GetComponent<SkillBook>();
        if (book == null) return;

        if (skillIndex >= 0)
        {
            book.UnlockSkill(skillIndex);
            Debug.Log($"[SKILL UNLOCK PICKUP] Unlock by index {skillIndex}");
        }
        else if (!string.IsNullOrEmpty(skillToUnlock))
        {
            book.UnlockSkillByName(skillToUnlock);
            Debug.Log($"[SKILL UNLOCK PICKUP] Unlock by name {skillToUnlock}");
        }
        else
        {
            Debug.LogWarning("[SKILL UNLOCK PICKUP] ยังไม่ได้ตั้ง skillToUnlock หรือ skillIndex");
        }

        Destroy(gameObject);
    }
}
