using UnityEngine;

public class UIAudioManeger : MonoBehaviour
{
   public AudioSource audioSource;
   public AudioClip buttonClickClip;

    public void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickClip != null)
        {
            audioSource.PlayOneShot(buttonClickClip);
        }
    }
}
