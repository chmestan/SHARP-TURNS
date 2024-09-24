using UnityEngine;

public class MusicManagerMain : MonoBehaviour
{
    [SerializeField] private AudioSource inGameMusicSource; 

    void Start()
    {
        if (inGameMusicSource != null && !inGameMusicSource.isPlaying)
        {
            inGameMusicSource.Play();
        }
    }
}
