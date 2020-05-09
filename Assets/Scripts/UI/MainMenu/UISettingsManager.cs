using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISettingsManager : MonoBehaviour
{
    [SerializeField]
    Text sensitivitySliderText;
    [SerializeField]
    Slider sensitivitySlider;

    float sensitivity = 5;
    public void InitializeMenu()
    {
        if (PlayerPrefs.HasKey("Sensitivity"))
            sensitivitySlider.value = PlayerPrefs.GetFloat("Sensitivity");
    }
    public void SetSensitivity()
    {
        sensitivity = sensitivitySlider.value;
        sensitivitySliderText.text = "Sensitivity : " + sensitivity;
    }

    public void SaveValues()
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        if (Camera.main.GetComponent<CameraController>())
        {
            Camera.main.GetComponent<CameraController>().sens = sensitivity;
        }
        PlayerPrefs.Save();
    }
}
