using UnityEngine;
using UnityEngine.UI;

public class ToggleScript : MonoBehaviour
{

    Toggle myToggle;
    public int id;
    Settings settings;

    public Settings.LABELS label;
    string _label;

    void Start()
    {
        settings = FindObjectOfType<Settings>();
        settings.showSettings += CheckToggledOnStart;
        myToggle = GetComponent<Toggle>();
        _label = Settings.getString(label);
    }

    public void CheckToggledOnStart()
    {
        myToggle.isOn = PlayerPrefs.GetInt(_label) == id;
    }

    public void OnChanged(bool value)
    {
        if (true)
            PlayerPrefs.SetInt(_label, id);
    }

}
