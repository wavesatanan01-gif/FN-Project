using UnityEngine;

public abstract class StatusEffect
{
    public string effectName;
    public float duration;   // ระยะเวลารวมของสถานะ (วินาที)
    public float elapsed;    // เวลาที่ผ่านไปแล้ว

    public bool allowStack = false;

    public bool IsFinished
    {
        get { return duration > 0 && elapsed >= duration; }
    }

    public virtual void OnApply(Character c)
    {
    }

    public virtual void OnRemove(Character c)
    {
    }

    public virtual void Tick(Character c, float dt)
    {
        if (duration > 0)
        {
            elapsed += dt;
        }
    }
}
