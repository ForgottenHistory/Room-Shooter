using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UISettingsManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////
    
    [SerializeField] Text sensitivitySliderText = null;
    [SerializeField] Slider sensitivitySlider = null;

    ///////////////////////////////////////////////////////////////////////////////
    // AUDIO SETTINGS
    ///////////////////////////////////////////////////////////////////////////////

    [Header("Audio")]
    public AudioMixer mixer = null;

    public Slider masterVolumeSlider = null;
    public Text masterVolumeText = null;

    public Slider musicVolumeSlider = null;
    public Text musicVolumeText = null;

    ///////////////////////////////////////////////////////////////////////////////

    float sensitivity = 5;
    
    ///////////////////////////////////////////////////////////////////////////////

    public void InitializeMenu()
    {
        ///////////////////////////////////////////////////////////////////////////////
        
        masterVolumeText.text = "Master Volume: " + PlayerPrefs.GetFloat( "MasterVolume" ).ToString();
        masterVolumeSlider.value = PlayerPrefs.GetFloat( "MasterVolume" );

        musicVolumeText.text = "Music volume: " + PlayerPrefs.GetFloat( "MusicVolume" ).ToString();
        musicVolumeSlider.value = PlayerPrefs.GetFloat( "MusicVolume" );
        
        ///////////////////////////////////////////////////////////////////////////////

        sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity", 5.0f);
        
        ///////////////////////////////////////////////////////////////////////////////
    }

    ///////////////////////////////////////////////////////////////////////////////

    public void SetMasterVolumeLevel()
    {
        mixer.SetFloat( "MasterVolume", Mathf.Log10( masterVolumeSlider.value ) * 20 );
        masterVolumeText.text = "Master Volume: " + ( Mathf.Round( masterVolumeSlider.value * 100f ) / 100f ).ToString();
        float saveValue = Mathf.Round( masterVolumeSlider.value * 100f ) / 100f;
        if ( saveValue <= 0 )
            saveValue = 0.001f;
        PlayerPrefs.SetFloat( "MasterVolume", saveValue );
    }
    
    ///////////////////////////////////////////////////////////////////////////////

    public void SetMusicVolumeLevel()
    {
        mixer.SetFloat( "MusicVolume", Mathf.Log10( musicVolumeSlider.value ) * 20 );
        musicVolumeText.text = "Music volume: " + ( Mathf.Round( musicVolumeSlider.value * 100f ) / 100f ).ToString();
        float saveValue = Mathf.Round( musicVolumeSlider.value * 100f ) / 100f;
        if ( saveValue <= 0 )
            saveValue = 0.001f;
        PlayerPrefs.SetFloat( "MusicVolume", saveValue );
    }
    
    ///////////////////////////////////////////////////////////////////////////////

    public void SetSensitivity()
    {
        sensitivity = sensitivitySlider.value;
        sensitivitySliderText.text = "Sensitivity : " + sensitivity;
    }
    
    ///////////////////////////////////////////////////////////////////////////////

    public void SaveValues()
    {
        PlayerPrefs.SetFloat( "Sensitivity", sensitivity );
        if ( Camera.main.GetComponent<CameraController>() )
        {
            Camera.main.GetComponent<CameraController>().sens = sensitivity;
        }
    }
    
    ///////////////////////////////////////////////////////////////////////////////
}
