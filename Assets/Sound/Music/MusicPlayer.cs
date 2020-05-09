using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EMusicPlayerMode
{
    MainMenu,
    InGame
}

public class MusicPlayer : MonoBehaviour
{
    //plays random music
    //music chosen in unity editor

    public List<AudioClip> music = new List<AudioClip>();
    AudioSource audioSource;
    float length;
    bool active = false;

    [SerializeField]
    EMusicPlayerMode mode;
    public void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        if( mode == EMusicPlayerMode.MainMenu )
        {
            audioSource.clip = music[ 0 ];
        }
        else if ( mode == EMusicPlayerMode.InGame )
        {
            audioSource.clip = music[ Random.Range( 0, music.Count ) ];
        }
        length = audioSource.clip.length;
        active = true;
        audioSource.Play();
    }
    private void Update()
    {
        if (active)
        {
            //length is a timer for when next song to play
            length -= Time.deltaTime;
            if (length <= 0)
            {
                if ( mode == EMusicPlayerMode.MainMenu )
                {
                    audioSource.clip = music[ 0 ];
                }
                else if ( mode == EMusicPlayerMode.InGame )
                {
                    audioSource.clip = music[ Random.Range( 0, music.Count ) ];
                }
                length = audioSource.clip.length;
                audioSource.Play();
            }
        }
    }
    //test for button
    public void NewSong()
    {
        audioSource.clip = music[Random.Range(0, music.Count)];
        length = audioSource.clip.length;
        audioSource.Play();
    }

    public void SetState(bool state)
    {
        active = state;

        if ( state == true )
            audioSource.Play();
        if (state == false )
            audioSource.Pause();
    }
}
