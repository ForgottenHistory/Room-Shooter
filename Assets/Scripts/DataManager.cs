using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct SMisc_InitValues
{
    public int coinsAmount; 
}

public class DataManager : MonoBehaviour
{
    ///////////////////////////////////////////////////////////////////////////////
    
    public static DataManager singleton;

    public PlayerController_InitValues playerController_Values;
    public SMisc_InitValues misc_Values;

    public AudioMixer mixer = null;
    
    ///////////////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        if ( singleton == null )
        {
            DontDestroyOnLoad( gameObject );
            singleton = this;
        }
        else if ( singleton != this )
        {
            Destroy( gameObject );
        }

        if ( PlayerPrefs.HasKey( "Has_DataValues" ) == false )
        {
            SetDefaultValues();
            PlayerPrefs.SetString( "Has_DataValues", "true" );
        }
        else
            LoadValues();


    }

    private void Start()
    {
        ///////////////////////////////////////////////////////////////////////////////
        // AUDIO
        ///////////////////////////////////////////////////////////////////////////////

        if ( PlayerPrefs.HasKey( "MasterVolume" ) == true )
            mixer.SetFloat( "MasterVolume", Mathf.Log10( PlayerPrefs.GetFloat( "MasterVolume" ) ) * 20 );
        else
            PlayerPrefs.SetFloat( "MasterVolume", 1.0f );

        if ( PlayerPrefs.HasKey( "MusicVolume" ) == true )
            mixer.SetFloat( "MusicVolume", Mathf.Log10( PlayerPrefs.GetFloat( "MusicVolume" ) ) * 20 );
        else
            PlayerPrefs.SetFloat( "MusicVolume", 1.0f );
    }

    ///////////////////////////////////////////////////////////////////////////////

    void LoadValues()
    {
        // load stuff from file
        BinaryFormatter bf = new BinaryFormatter();
        if ( File.Exists( Application.persistentDataPath + "/player_values.dat" ) )
        {
            FileStream file = File.Open(Application.persistentDataPath + "/player_values.dat", FileMode.Open );
            playerController_Values = ( PlayerController_InitValues)bf.Deserialize( file );
            file.Close();
        }
        if ( File.Exists( Application.persistentDataPath + "/misc_values.dat" ) )
        {
            FileStream file = File.Open(Application.persistentDataPath + "/misc_values.dat", FileMode.Open );
            misc_Values = (SMisc_InitValues)bf.Deserialize( file );
            file.Close();
        }
    }

    ///////////////////////////////////////////////////////////////////////////////

    public void SaveValues()
    {
        //save to file
        //binaryformatter to save lists and etc

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create( Application.persistentDataPath + "/player_values.dat" );
        bf.Serialize( file, playerController_Values );
        file.Close();

        file = File.Create( Application.persistentDataPath + "/misc_values.dat" );
        bf.Serialize( file, misc_Values );
        file.Close();
    }

    ///////////////////////////////////////////////////////////////////////////////

    public bool SetValue( string value, int amount )
    {
        switch ( value )
        {
            case "ImmunityTimeUpgrade":
                if ( playerController_Values.damageShieldTime + amount > -6 )
                {
                    playerController_Values.damageShieldTime += amount;
                    return true;
                }
                else
                    return false;

            case "FallSpeedUpgrade":
                if( playerController_Values.fallSpeed + amount > -6 )
                {
                    playerController_Values.fallSpeed += amount;
                    return true;
                }
                else
                    return false;

            case "JumpLimitUpgrade":
                if( playerController_Values.jumpLimit + amount > -6 )
                {
                    playerController_Values.jumpLimit += amount;
                    return true;
                }
                else
                    return false;

            case "JumpHeightUpgrade":
                if( playerController_Values.jumpHeight + amount > -6 )
                {
                    playerController_Values.jumpHeight += amount;
                    return true;
                }
                else
                    return false;

            case "SpeedUpgrade":
                if( playerController_Values.movementSpeed + amount > -6 )
                {
                    playerController_Values.movementSpeed += amount;
                    return true;
                }
                else
                    return false;

            case "JetpackChargeUpgrade":
                if( playerController_Values.flyingFill + amount > -6 )
                {
                    playerController_Values.flyingFill += amount;
                    return true;
                }
                else
                    return false;

            case "JetpackUseRateUpgrade":
                if( playerController_Values.flyingUseRate + amount > -6 )
                {
                    playerController_Values.flyingUseRate += amount;
                    return true;
                }
                else
                    return false;

            case "JetpackRecovoryRateUpgrade":
                if( playerController_Values.flyingRecovoryRate + amount > -6 )
                {
                    playerController_Values.flyingRecovoryRate += amount;
                    return true;
                }
                else
                    return false;

            case "JetpackPowerUpgrade":
                if( playerController_Values.flyingPower + amount > -5 )
                {
                    playerController_Values.flyingPower += amount;
                    return true;
                }
                else
                    return false;

            default:
                return false;
        }
    }

    ///////////////////////////////////////////////////////////////////////////////

    public int GetValue( string value )
    {
        switch ( value )
        {
            case "ImmunityTimeUpgrade":
                return playerController_Values.damageShieldTime;
            case "FallSpeedUpgrade":
                return playerController_Values.fallSpeed;
            case "JumpLimitUpgrade":
                return playerController_Values.jumpLimit;
            case "JumpHeightUpgrade":
                return playerController_Values.jumpHeight;
            case "SpeedUpgrade":
                return playerController_Values.movementSpeed;
            case "JetpackChargeUpgrade":
                return playerController_Values.flyingFill;
            case "JetpackUseRateUpgrade":
                return playerController_Values.flyingUseRate;
            case "JetpackRecovoryRateUpgrade":
                return playerController_Values.flyingRecovoryRate;
            case "JetpackPowerUpgrade":
                return playerController_Values.flyingPower;
            default:
                break;
        }

        return 0;
    }

    ///////////////////////////////////////////////////////////////////////////////

    public void SetDefaultValues()
    {
        playerController_Values.damageShieldTime = 1;
        playerController_Values.fallSpeed = 1;
        playerController_Values.flyingFill = 1;
        playerController_Values.flyingPower = 1;
        playerController_Values.flyingRecovoryRate = 1;
        playerController_Values.flyingUseRate = 1;
        playerController_Values.jumpHeight = 1;
        playerController_Values.movementSpeed = 1;
        playerController_Values.jumpLimit = 1;

        misc_Values.coinsAmount = 0;

        SaveValues();
    }

    ///////////////////////////////////////////////////////////////////////////////

    public void DeleteData()
    {
        File.Delete( Application.persistentDataPath + "/player_values.dat" );
        File.Delete( Application.persistentDataPath + "/misc_values.dat" );
        SaveValues();
    }

    ///////////////////////////////////////////////////////////////////////////////
}