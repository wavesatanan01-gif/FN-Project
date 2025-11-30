using UnityEngine;

public class ClickSFX : MonoBehaviour
{
    public AudioClip clip;

   
    private void OnMouseDown()
    {
        if (clip != null && SoundManager.instance != null)
            SoundManager.instance.PlaySFX(clip);  
    }
}
