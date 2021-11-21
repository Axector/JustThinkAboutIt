using UnityEngine;

public class Music : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = PlayerPrefs.GetFloat("music_volume", 1f);
    }

    public void SetMusicVolume(float volume)
    {
        audioSource.volume = volume;
    }
}
