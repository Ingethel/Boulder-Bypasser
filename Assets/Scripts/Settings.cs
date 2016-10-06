using UnityEngine;
using System;

public class Settings : MonoBehaviour
{

    public enum LABELS {
        MOVEMENT_SETTIGNS,
        ACCELERATION_SPEED,
        CAMERA_SETTINGS,
        CHARACTER_TRANSPARENCY,
        REVERSE_CONTROL_X,
        REVERSE_CONTROL_Y,
        HIGHSCORE,
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
            case LABELS.REVERSE_CONTROL_X:
                _return = "Reverse Control X";
                break;
            case LABELS.REVERSE_CONTROL_Y:
                _return = "Reverse Control Y";
                break;
            case LABELS.HIGHSCORE:
                _return = "High Score";
                break;
            default:
                break;
        }
        return _return;
    }
    
    public event Action showSettings;

    void Awake()
    {
        ReadSettings();
    }
    
    public void ReadSettings()
    {
        if (PlayerPrefs.GetInt(getString(LABELS.MOVEMENT_SETTIGNS)) == 0)
        {
            PlayerPrefs.SetInt(getString(LABELS.MOVEMENT_SETTIGNS), 1);
        }
        
        if (PlayerPrefs.GetInt(getString(LABELS.ACCELERATION_SPEED)) == 0)
        {
            PlayerPrefs.SetInt(getString(LABELS.ACCELERATION_SPEED), 2);
        }
        
        if (PlayerPrefs.GetInt(getString(LABELS.CAMERA_SETTINGS)) == 0)
        {
            PlayerPrefs.SetInt(getString(LABELS.CAMERA_SETTINGS), 1);
        }
        
        if (PlayerPrefs.GetInt(getString(LABELS.REVERSE_CONTROL_X)) == 0) {
            PlayerPrefs.SetInt(getString(LABELS.REVERSE_CONTROL_X), -1);
        }

        if (PlayerPrefs.GetInt(getString(LABELS.REVERSE_CONTROL_Y)) == 0)
        {
            PlayerPrefs.SetInt(getString(LABELS.REVERSE_CONTROL_Y), -1);
        }
    }

    public void OnShow()
    {
        if (showSettings != null)
            showSettings();
    }
}
