using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public struct SMisc_InitValues
{
    public int coinsAmount; 
}

public class DataManager : MonoBehaviour
{
    //singleton
    public static DataManager singleton;

    public PlayerController_InitValues playerController_Values;
    public SMisc_InitValues misc_Values;

    private void Awake()
    {
        if (singleton == null)
        {
            DontDestroyOnLoad(gameObject);
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        if(PlayerPrefs.HasKey("Has_DataValues") == false)
        {
            SetDefaultValues();
            PlayerPrefs.SetString("Has_DataValues", "true");
        }
        else
            LoadValues();
    }

    void LoadValues()
    {
        // load stuff from file
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/player_values.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/player_values.dat", FileMode.Open);
            playerController_Values = (PlayerController_InitValues)bf.Deserialize(file);
            file.Close();
        }
        if (File.Exists(Application.persistentDataPath + "/misc_values.dat"))
        {
            FileStream file = File.Open(Application.persistentDataPath + "/misc_values.dat", FileMode.Open);
            misc_Values = (SMisc_InitValues)bf.Deserialize(file);
            file.Close();
        }
    }
    public void SaveValues()
    {
        //save to file
        //binaryformatter to save lists and etc

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/player_values.dat");
        bf.Serialize(file, playerController_Values);
        file.Close();

        file = File.Create(Application.persistentDataPath + "/misc_values.dat");
        bf.Serialize(file, misc_Values);
        file.Close();
    }

    public void SetValue(string value, int amount)
    {
        switch (value)
        {
            case "ImmunityTimeUpgrade":
                playerController_Values.damageShieldTime += amount;
                break;
            case "FallSpeedUpgrade":
                    playerController_Values.fallSpeed += amount;
                break;
            case "JumpLimitUpgrade":
                    playerController_Values.jumpLimit += amount;
                break;
            case "JumpHeightUpgrade":
                    playerController_Values.jumpHeight += amount;
                break;
            case "SpeedUpgrade":
                    playerController_Values.movementSpeed += amount;
                break;
            case "JetpackChargeUpgrade":
                    playerController_Values.flyingFill += amount;
                break;
            case "JetpackUseRateUpgrade":
                    playerController_Values.flyingUseRate += amount;
                break;
            case "JetpackRecovoryRateUpgrade":
                    playerController_Values.flyingRecovoryRate += amount;
                break;
            case "JetpackPowerUpgrade":
                    playerController_Values.flyingPower += amount;
                break;
            default:
                break;
        }
    }

    public int GetValue(string value)
    {
        switch (value)
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

    public void DeleteData()
    {
        File.Delete(Application.persistentDataPath + "/player_values.dat");
        File.Delete(Application.persistentDataPath + "/misc_values.dat");
        SaveValues();
    }
}