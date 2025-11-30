using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public AudioClip clickSound;

    public void PlayClickSound()
    {
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySFX(clickSound);
    }
}