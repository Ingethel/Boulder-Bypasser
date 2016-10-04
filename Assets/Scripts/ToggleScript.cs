using UnityEngine;
using UnityEngine.UI;
using System;

public class ToggleScript : MonoBehaviour
{

    Toggle myToggle;
    ToggleGroup myGroup = null;
    public int id;
    Settings settings;

    public Settings.LABELS label;
    string _label;

    public event Action toggleListeners;
    public Toggle parent;

    void Start()
    {
        settings = FindObjectOfType<Settings>();
        settings.showSettings += CheckToggledOnStart;
        myToggle = GetComponent<Toggle>();
        _label = Settings.getString(label);
        myGroup = myToggle.group;

        if (parent != null)
            parent.GetComponent<ToggleScript>().toggleListeners += checkInteractivity;
    }

    public void CheckToggledOnStart()
    {
        myToggle.isOn = PlayerPrefs.GetInt(_label) == id;
        if (parent != null) checkInteractivity();
    }

    public void OnChanged(bool value)
    {
        if (value)
            PlayerPrefs.SetInt(_label, id);
        else if(myGroup == null)
            PlayerPrefs.SetInt(_label, -1);

        if (toggleListeners != null)
            toggleListeners();
    }

    public void checkInteractivity()
    {
        myToggle.interactable = parent.isOn;
    }
}
