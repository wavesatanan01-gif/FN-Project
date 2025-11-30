using UnityEngine;

public class KeepUpright : MonoBehaviour
{
    void LateUpdate()
    {
        // ล็อกให้หมุนได้แค่แกน Y (หันซ้ายขวา แต่ไม่ตีลังกา)
        Vector3 euler = transform.eulerAngles;
        euler.x = 0f;
        euler.z = 0f;
        transform.eulerAngles = euler;
    }
}
