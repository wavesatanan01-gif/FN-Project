using UnityEngine;

public class KeepPlayerUpright : MonoBehaviour
{
    void LateUpdate()
    {
        // ให้ตัวยืนตรงตลอด ไม่เอียง X/Z
        Vector3 euler = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, euler.y, 0f);
    }
}
