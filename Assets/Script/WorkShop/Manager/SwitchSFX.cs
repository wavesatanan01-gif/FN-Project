using UnityEngine;

public class SwitchSFX : MonoBehaviour
{
    public AudioClip switchSound;

    private void OnMouseDown()
    {
        if (SoundManager.instance != null && switchSound != null)
        {
            SoundManager.instance.PlaySFX(switchSound);
            Debug.Log("Switch clicked!");
        }
    }
}