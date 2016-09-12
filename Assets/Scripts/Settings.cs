using UnityEngine;
using System;

public class Settings : MonoBehaviour
{

    int movementSettings;
    int accelerometerSpeed;

    public static string movement = "Movement Settings";
    public static string accelerometerSensitivity = "Accelerometer Speed";
    
    public event Action showSettings;

    void Awake()
    {
        ReadSettings();
    }
    
    public void ReadSettings()
    {
        movementSettings = PlayerPrefs.GetInt(movement);
        accelerometerSpeed = PlayerPrefs.GetInt(accelerometerSensitivity);
        if (accelerometerSpeed == 0)
        {
            accelerometerSpeed = 2;
            PlayerPrefs.SetInt(accelerometerSensitivity, accelerometerSpeed);
        }
        if (movementSettings == 0) {
            movementSettings = 1;
            PlayerPrefs.SetInt(movement, movementSettings);
        }
    }
    
    public void OnShow()
    {
        if (showSettings != null)
            showSettings();
    }
}
