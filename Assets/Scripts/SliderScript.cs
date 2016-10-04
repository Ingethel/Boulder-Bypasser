using UnityEngine;
using UnityEngine.UI;

public class SliderScript : MonoBehaviour {

    public enum TYPE { INT, FLOAT}

    public TYPE sliderValueType;

    Slider slider;
    Settings settings;

    public Settings.LABELS label;
    string _label;

    public Toggle parent;

    void Start ()
    {
        settings = FindObjectOfType<Settings>();
        settings.showSettings += CheckSliderOnStart;
        slider = GetComponent<Slider>();
        _label = Settings.getString(label);

        if (parent != null)
            parent.GetComponent<ToggleScript>().toggleListeners += checkInteractivity;
    }

    public void CheckSliderOnStart()
    {
        if(sliderValueType == TYPE.INT)
            slider.value = PlayerPrefs.GetInt(_label);
        else if(sliderValueType == TYPE.FLOAT)
            slider.value = PlayerPrefs.GetFloat(_label);

        if (parent != null) checkInteractivity();

    }

    public void OnChanged(float value)
    {
        if (sliderValueType == TYPE.INT)
            PlayerPrefs.SetInt(_label, (int)value);
        else if (sliderValueType == TYPE.FLOAT)
            PlayerPrefs.SetFloat(_label, value);
    }

    public void checkInteractivity()
    {
        slider.interactable = parent.isOn;
    }
}
