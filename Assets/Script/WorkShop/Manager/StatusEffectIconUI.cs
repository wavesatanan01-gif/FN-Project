using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusEffectIconUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI timerText;

    private StatusEffect effect;
    private System.Func<StatusEffect, Sprite> getIconFunc;

    public void Init(StatusEffect eff, System.Func<StatusEffect, Sprite> iconResolver)
    {
        effect = eff;
        getIconFunc = iconResolver;
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        if (effect == null) return;

        // ตั้ง icon ตามชื่อ effect
        if (iconImage != null && getIconFunc != null)
        {
            Sprite s = getIconFunc(effect);
            if (s != null) iconImage.sprite = s;
        }

        // ตั้งเวลา
        if (timerText != null && effect.duration > 0)
        {
            float remain = Mathf.Max(0f, effect.duration - effect.elapsed);
            timerText.text = remain.ToString("0.0");
        }
        else if (timerText != null)
        {
            timerText.text = "";
        }
    }
}
