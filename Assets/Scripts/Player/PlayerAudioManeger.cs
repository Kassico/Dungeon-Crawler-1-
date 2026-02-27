using UnityEngine;

public class PlayerAudioManeger : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource footstepSource;
    public AudioSource attackSource;
    public AudioSource dmgSource;
    public AudioSource levelUpSource;
    public AudioSource deathSource;
    public AudioSource hitSource;
    public AudioSource dashSource;

    [Header("Clips")]
    public AudioClip[] footstepClips;
    public AudioClip[] dmgClips;

    [Header("Other Sounds")]

    public AudioClip attackClip;
    //public AudioClip dmgClip;
    public AudioClip levelUpClip;
    public AudioClip deathClip;
    public AudioClip hitClip;
    public AudioClip dashClip;


    //FoorSteps
    public void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;
        
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];

        footstepSource.PlayOneShot(clip);
    }


    //Attack
    public void PlayAttack()
    {
        if (attackClip == null) return;
        attackSource.PlayOneShot(attackClip);


    }

    // Take Damage
    public void PlayTakeDamage()
    {
        if(dmgClips.Length == 0) return;
        AudioClip clip = dmgClips[Random.Range(0, dmgClips.Length)];

        dmgSource.PlayOneShot(clip);
    }

    // Level Up
    public void PlayLevelUp()
    {
        if (levelUpClip == null) return;
        levelUpSource.PlayOneShot(levelUpClip);
    }

    // Death
    public void PlayDeath()
    {
        if (deathClip == null) return;
        deathSource.PlayOneShot(deathClip);
    }
    // Hit
    public void PlayHit()
    {
        if (hitClip == null) return;
        hitSource.PlayOneShot(hitClip);
    }
    //Dash
    public void PlayDash()
    {
        if (dashClip == null) return;
        dashSource.PlayOneShot(dashClip);
    }
}
