using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeDuration = 2f;
    private List<AudioClip> currentPlaylist = new List<AudioClip>();
    private int currentTrackIndex = 0;
    private Coroutine fadeCoroutine;
    private Coroutine playlistCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayPlaylist(List<AudioClip> playlist)
    {
        if (playlistCoroutine != null)
        {
            StopCoroutine(playlistCoroutine);
        }

        currentPlaylist = playlist;
        currentTrackIndex = 0;
        playlistCoroutine = StartCoroutine(PlayAudioTracks());
    }

    private IEnumerator PlayAudioTracks()
    {
        while (true)
        {
            AudioClip currentTrack = currentPlaylist[currentTrackIndex];

            yield return StartCoroutine(CrossfadeAudio(currentTrack));

            yield return new WaitForSeconds(currentTrack.length);

            currentTrackIndex = (currentTrackIndex + 1) % currentPlaylist.Count;
        }
    }

    private IEnumerator CrossfadeAudio(AudioClip newClip)
    {
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.clip = newClip;
        audioSource.Play();
        audioSource.volume = 0;

        while (audioSource.volume < startVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = startVolume;
    }
}
