using System.Collections;
using UnityEngine;

public class HazardManager : MonoBehaviour
{
    public static HazardManager I;

    [Header("FX")]
    public GameObject poisonGasFX;

    [Header("Damage Settings")]
    public int damagePerTick = 10;
    public float tickRate = 1f;

    [Header("Debug State")]
    public bool gasActive = false;

    private Player player;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();

        if (poisonGasFX != null)
            poisonGasFX.SetActive(false); 
    }

    public void StartGas()
    {
        if (gasActive) return;

        gasActive = true;
        Debug.Log($"[HAZARD] Poison Gas STARTED. Damage {damagePerTick} every {tickRate} sec.");

        if (poisonGasFX != null)
            poisonGasFX.SetActive(true);

        StartCoroutine(GasRoutine());
    }
    public void StopGas()
    {
        gasActive = false;

        if (poisonGasFX != null)
            poisonGasFX.SetActive(false);

        Debug.Log("[HAZARD] Poison Gas STOPPED.");
    }

    private IEnumerator GasRoutine()
    {
        while (gasActive)
        {
            if (player != null && player.health > 0)
            {
                player.TakePureDamage(damagePerTick);
                Debug.Log($"[HAZARD] Apply GAS damage {damagePerTick} to Player (pure). HP now = {player.health}");
            }
            else
            {
                StopGas();
                yield break;
            }

            yield return new WaitForSeconds(tickRate);
        }
    }

}



