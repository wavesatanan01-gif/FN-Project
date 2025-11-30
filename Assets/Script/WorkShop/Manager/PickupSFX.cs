using UnityEngine;

public class PickupSFX : MonoBehaviour
{
    public AudioClip clip;
    public int scoreValue = 1;     

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (clip != null && SoundManager.instance != null)
            SoundManager.instance.PlaySFX(clip);   

      
        if (GameManager.instance != null)
            GameManager.instance.AddScore(scoreValue); 

        Destroy(gameObject); 
    }
}
