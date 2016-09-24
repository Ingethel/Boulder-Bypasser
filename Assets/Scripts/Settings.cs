using UnityEngine;
using System;

public class Settings : MonoBehaviour
{

    public enum LABELS {
        MOVEMENT_SETTIGNS,
        ACCELERATION_SPEED,
        CAMERA_SETTINGS,
        CHARACTER_TRANSPARENCY
    }

    public static string getString(LABELS label) {
        string _return = null;
        switch (label) {
            case LABELS.MOVEMENT_SETTIGNS:
                _return = "Movement Settings";
                break;
            case LABELS.ACCELERATION_SPEED:
                _return = "Accelerometer Speed";
                break;
            case LABELS.CAMERA_SETTINGS:
                _return = "Camera Settings";
                break;
            case LABELS.CHARACTER_TRANSPARENCY:
                _return = "Character Transparency";
                break;
            default:
                break;
        }
        return _return;

    }

    int movementSettings;
    int accelerometerSpeed;
    int cameraSettings;
    
    public event Action showSettings;

    void Awake()
    {
        ReadSettings();
    }
    
    public void ReadSettings()
    {
        movementSettings = PlayerPrefs.GetInt(getString(LABELS.MOVEMENT_SETTIGNS));
        if (movementSettings == 0)
        {
            movementSettings = 1;
            PlayerPrefs.SetInt(getString(LABELS.MOVEMENT_SETTIGNS), movementSettings);
        }

        accelerometerSpeed = PlayerPrefs.GetInt(getString(LABELS.ACCELERATION_SPEED));
        if (accelerometerSpeed == 0)
        {
            accelerometerSpeed = 2;
            PlayerPrefs.SetInt(getString(LABELS.ACCELERATION_SPEED), accelerometerSpeed);
        }

        cameraSettings = PlayerPrefs.GetInt(getString(LABELS.CAMERA_SETTINGS));
        if (cameraSettings == 0)
        {
            cameraSettings = 1;
            PlayerPrefs.SetInt(getString(LABELS.CAMERA_SETTINGS), cameraSettings);
        }
    }

    public void OnShow()
    {
        if (showSettings != null)
            showSettings();
    }
}
