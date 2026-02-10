using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip backgroundMusic; // Assign your audio clip in the Inspector

    void Awake()
    {
        // Check if another AudioManager already exists
        GameObject[] objs = GameObject.FindGameObjectsWithTag("MusicPlayer");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            // If it's the first one, make it persistent
            DontDestroyOnLoad(this.gameObject);
            this.gameObject.tag = "MusicPlayer";
            audioSource = GetComponent<AudioSource>();

            // Play the music if it's not already playing
            if (!audioSource.isPlaying)
            {
                audioSource.clip = backgroundMusic;
                audioSource.Play();
            }
        }
    }
}