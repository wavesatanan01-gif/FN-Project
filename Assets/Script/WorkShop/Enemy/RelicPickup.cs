using UnityEngine;

public class RelicPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // เพิ่มเลเวลความยาก
        if (DifficultyManager.I != null)
        {
            DifficultyManager.I.LevelUp();
        }

        // บอกระบบประตูว่ามีการเก็บ Relic แล้ว 1 ชิ้น
        var portalOpener = FindObjectOfType<PortalGateOpener>();
        if (portalOpener != null)
        {
            portalOpener.OnRelicCollected();
        }

        // ⭐ บอก SkillBook ว่าเก็บ relic แล้ว
        SkillBook book = other.GetComponent<SkillBook>();
        if (book != null)
        {
            book.relicCount++;

            if (book.relicCount == 1)
                book.UnlockSkillByName("Speed Boost");
            else if (book.relicCount == 2)
                book.UnlockSkillByName("Fireball");
            // else if (book.relicCount == 3)
                // book.UnlockSkillByName("Defense Buff");
        }
        Destroy(gameObject);


    Debug.Log("<color=#00FFFF>[Relic]</color> Player picked a relic.");

        Destroy(gameObject);
    }

}
