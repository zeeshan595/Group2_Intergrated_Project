using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] music;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if (!GetComponent<AudioSource>())
            gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!audio.isPlaying)
        {
            audio.clip = music[Random.Range(0, music.Length)];
            audio.Play();
        }
    }
}