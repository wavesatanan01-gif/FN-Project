using UnityEngine;

[DefaultExecutionOrder(1000)]  // ให้รันหลังสคริปต์อื่น
public class PortalGateEffectFix : MonoBehaviour
{
    [Header("Shader Values")]
    [Range(0f, 1f)]
    public float alpha = 0.6f;          // ความทึบของวงประตู
    public float portalFade = 1f;       // ค่า fade หลักของ portal

    private void OnEnable()
    {
        ApplyShaderValues();
    }

    private void Start()
    {
        ApplyShaderValues();
    }

    private void ApplyShaderValues()
    {
        // กวาด Renderer ทุกตัวใต้ประตูเปิด
        var renderers = GetComponentsInChildren<Renderer>(true);
        foreach (var r in renderers)
        {
            var mats = r.materials;
            for (int i = 0; i < mats.Length; i++)
            {
                var m = mats[i];

                // เผื่อชื่อ property มีหรือไม่มีขีดล่าง
                if (m.HasProperty("_Alpha"))
                    m.SetFloat("_Alpha", alpha);
                if (m.HasProperty("Alpha"))
                    m.SetFloat("Alpha", alpha);

                if (m.HasProperty("_PortalFade_ScriptVar"))
                    m.SetFloat("_PortalFade_ScriptVar", portalFade);
                if (m.HasProperty("PortalFade_ScriptVar"))
                    m.SetFloat("PortalFade_ScriptVar", portalFade);
            }
        }

        Debug.Log("[PORTAL FX] Forced shader values applied.");
    }
}
