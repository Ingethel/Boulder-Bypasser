using UnityEngine;

public class CameraManager : MonoBehaviour {

    public Camera TPS_Camera, FPS_Camera;
    GameManager manager;

	void Start () {
        manager = FindObjectOfType<GameManager>();
        manager.InGameEvent += UpdateCamera;
	}

    public void UpdateCamera() {
        int enabledCamera = PlayerPrefs.GetInt(Settings.getString(Settings.LABELS.CAMERA_SETTINGS));
        if (enabledCamera == 1)
        {
            TPS_Camera.enabled = true;
            FPS_Camera.enabled = false;
        }
        else if (enabledCamera == 2)
        {
            TPS_Camera.enabled = false;
            FPS_Camera.enabled = true;
        }

    }
}
