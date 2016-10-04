using UnityEngine;
using System.Collections;

public class GameInput : MonoBehaviour
{

    public Transform marker;
    Camera _camera;
    PlayerMovement player;
    GameManager manager;

    public int callibrationFrames;
    public bool callibrating = false;
    Matrix4x4 calibrationMatrix;

    float accelerometerSpeed;
    bool accelerometerInput;
    int inverseControl;

    void Start()
    {
        _camera = Camera.main;
        player = FindObjectOfType<PlayerMovement>();
        manager = FindObjectOfType<GameManager>();
        manager.InGameEvent += ReadSettings;
    }

    void Update()
    {
        if (manager.currentState == GameManager.GameState.INGAME)
        {
            if (accelerometerInput)
                ReadAccelerometer();
            else
                ReadTouch();
        }
        ReadKeys();
    }

    void ReadTouch()
    {
        if (Input.GetMouseButton(0))
        {
            Collider2D _collider = Physics2D.OverlapPoint(Input.mousePosition);
            if (_collider == null)
            {
                Vector3 mousePosition;
                mousePosition = Input.mousePosition;
                mousePosition.z = 25;
                FollowPoint.follow2D(marker, _camera.ScreenToWorldPoint(mousePosition));
                player.UpdateFocus();
            }
        }

        if (Input.GetKey(KeyCode.Space))
            player.SpeedBoost();
        if (Input.GetKeyUp(KeyCode.Space))
            player.BoostCoolDown();
    }

    void ReadKeys()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (manager.currentState)
            {
                case GameManager.GameState.INGAME:
                    manager.Pause();
                    break;
                case GameManager.GameState.MENU:
                case GameManager.GameState.ENDGAME:
                case GameManager.GameState.PAUSE:
                    manager.ExitGame();
                    break;
                case GameManager.GameState.OPTIONS:
                    manager.ExitOptions();
                    break;
                case GameManager.GameState.CALIBRATING:
                default:
                    break;
            }
        }
    }

    void ReadAccelerometer()
    {
        if (!callibrating)
        {
            Vector3 dir = Vector3.zero;
            Vector3 acceleration = calibrationMatrix.MultiplyVector(Input.acceleration);

            dir.x = acceleration.x;
            dir.y = -acceleration.y;

            if (dir.sqrMagnitude > 1)
                dir.Normalize();

            dir *= Time.deltaTime;
            if (dir.x != 0.01 || dir.y != 0.01)
            {
                marker.Translate(dir * accelerometerSpeed * -inverseControl);
                player.UpdateFocus();
            }
        }
    }

    public void CalibrateAccelerometer()
    {
        StartCoroutine(CalibrateAccelerometerRoutine());
    }

    IEnumerator CalibrateAccelerometerRoutine()
    {
        callibrating = true;
        int i = 0;
        Vector3 sumAcceleration = Vector3.zero;

        while (i < callibrationFrames)
        {
            sumAcceleration += Input.acceleration;
            i++;
            yield return new WaitForEndOfFrame();
        }

        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), sumAcceleration);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
        calibrationMatrix = matrix.inverse;

        callibrating = false;
    }

    public void ReadSettings()
    {
        accelerometerInput = PlayerPrefs.GetInt(Settings.getString(Settings.LABELS.MOVEMENT_SETTIGNS)) == 1;
        accelerometerSpeed = PlayerPrefs.GetInt(Settings.getString(Settings.LABELS.ACCELERATION_SPEED)) * 10;
        inverseControl = PlayerPrefs.GetInt(Settings.getString(Settings.LABELS.REVERSE_CONTROL));
    }

}
