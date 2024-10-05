using UnityEngine;
using System.Collections.Generic;

public class RandomSound : MonoBehaviour
{
    [SerializeField] private List<AudioClip> clickSounds;
    [SerializeField] private AudioSource audioSource;  

    public void PlayRandomSound()
    {
        if (clickSounds.Count > 0 && audioSource != null)
        {
            int randomIndex = Random.Range(0, clickSounds.Count);
            audioSource.clip = clickSounds[randomIndex];

            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No AudioClips found or AudioSource is missing.");
        }
    }
}
