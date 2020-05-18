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
    ////////////////////////////////////////////////////////////////

    public List<AudioClip> music = new List<AudioClip>();
    [SerializeField] EMusicPlayerMode mode = EMusicPlayerMode.InGame;

    ////////////////////////////////////////////////////////////////

    AudioSource audioSource;
    float length = 0.0f;
    bool active = false;

    ////////////////////////////////////////////////////////////////

    public void Initialize()
    {
        ////////////////////////////////////////////////////////////////
        
        audioSource = GetComponent<AudioSource>();
        if( mode == EMusicPlayerMode.MainMenu )
        {
            audioSource.clip = music[ 0 ];
        }
        else if ( mode == EMusicPlayerMode.InGame )
        {
            audioSource.clip = music[ Random.Range( 0, music.Count ) ];
        }
        
        ////////////////////////////////////////////////////////////////

        length = audioSource.clip.length;
        active = true;
        audioSource.Play();
        
        ////////////////////////////////////////////////////////////////
    }

    ////////////////////////////////////////////////////////////////

    private void Update()
    {
        if ( active == true )
        {
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
    
    ////////////////////////////////////////////////////////////////

    public void NewSong()
    {
        audioSource.clip = music[ Random.Range( 0, music.Count ) ];
        length = audioSource.clip.length;
        audioSource.Play();

        DebugManager.GetInstance().Print( this.ToString(), "Playing new song: " + audioSource.clip.name + " Length: " + audioSource.clip.length );
    }
    
    ////////////////////////////////////////////////////////////////

    public void SetState(bool state)
    {
        active = state;

        if ( state == true )
            audioSource.Play();
        if ( state == false )
            audioSource.Pause();
    }
    
    ////////////////////////////////////////////////////////////////
}
