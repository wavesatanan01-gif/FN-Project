using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectIconEntry
{
    public string effectName;   
    public Sprite icon;        
}

public class StatusEffectUIManager : MonoBehaviour
{
    [Header("Target")]
    public Player targetPlayer;

    [Header("UI")]
    public Transform iconContainer; 
    public StatusEffectIconUI iconPrefab;

    [Header("Icon Mapping")]
    public List<EffectIconEntry> iconEntries = new List<EffectIconEntry>();

    private readonly Dictionary<StatusEffect, StatusEffectIconUI> iconByEffect
        = new Dictionary<StatusEffect, StatusEffectIconUI>();

    void Start()
    {
        if (targetPlayer == null)
        {
            targetPlayer = FindObjectOfType<Player>();
        }

        if (targetPlayer != null)
        {
            targetPlayer.OnEffectApplied += HandleEffectApplied;
            targetPlayer.OnEffectRemoved += HandleEffectRemoved;
        }
        else
        {
            Debug.LogWarning("[StatusEffectUI] Player not found");
        }
    }

    void OnDestroy()
    {
        if (targetPlayer != null)
        {
            targetPlayer.OnEffectApplied -= HandleEffectApplied;
            targetPlayer.OnEffectRemoved -= HandleEffectRemoved;
        }
    }

    private void HandleEffectApplied(StatusEffect effect)
    {
        Debug.Log($"[UI] HandleEffectApplied: {effect.effectName}");

        if (effect == null || iconPrefab == null || iconContainer == null)
            return;

        StatusEffect toReplace = null;
        StatusEffectIconUI existingIcon = null;

        foreach (var kvp in iconByEffect)
        {
            StatusEffect e = kvp.Key;

            if (e.effectName == effect.effectName && e.GetType() == effect.GetType())
            {
                toReplace = e;
                existingIcon = kvp.Value;
                break;
            }
        }

        if (existingIcon != null)
        {
            existingIcon.Init(effect, ResolveIcon);

            iconByEffect.Remove(toReplace);
            iconByEffect[effect] = existingIcon;

            return;
        }

        var icon = Instantiate(iconPrefab, iconContainer);
        icon.Init(effect, ResolveIcon);
        iconByEffect[effect] = icon;
    }


    private void HandleEffectRemoved(StatusEffect effect)
    {
        if (effect == null) return;

        if (iconByEffect.TryGetValue(effect, out var icon))
        {
            Destroy(icon.gameObject);
            iconByEffect.Remove(effect);
        }
    }

    private void Update()
    {
        foreach (var kvp in iconByEffect)
        {
            kvp.Value.UpdateVisual();
        }
    }

    private Sprite ResolveIcon(StatusEffect effect)
    {
        if (effect == null) return null;

        foreach (var entry in iconEntries)
        {
            if (entry.effectName == effect.effectName)
                return entry.icon;
        }

        return null;
    }
}
