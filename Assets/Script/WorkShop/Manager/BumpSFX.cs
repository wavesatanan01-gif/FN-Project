using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BumpSFX : MonoBehaviour
{
    public AudioClip clip;
    public float minImpact = 1.0f; 

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.relativeVelocity.magnitude < minImpact) return;

        if (clip != null && SoundManager.instance != null)
            SoundManager.instance.PlaySFX(clip);
    }
}
