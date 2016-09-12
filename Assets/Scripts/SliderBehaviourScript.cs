using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviourScript : MonoBehaviour {

    Slider slider;
    Settings settings;

	void Start ()
    {
        settings = FindObjectOfType<Settings>();
        settings.showSettings += CheckSliderOnStart;
        slider = GetComponent<Slider>();
	}
	
    public void CheckSliderOnStart()
    {
        slider.value = PlayerPrefs.GetInt(Settings.accelerometerSensitivity);
    }

    public void OnChanged(float value)
    {
        PlayerPrefs.SetInt(Settings.accelerometerSensitivity, (int)value);
    }
}
