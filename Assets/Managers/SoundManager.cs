using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource; // Dedicated AudioSource for sound effects

    private void Awake()
    {
        // Ensure there's only one instance of SoundManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Keep SoundManager across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Play a sound effect
    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip, volume);
        }
    }
}
