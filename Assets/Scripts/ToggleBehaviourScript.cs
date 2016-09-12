using UnityEngine;
using UnityEngine.UI;

public class ToggleBehaviourScript : MonoBehaviour
{

    Toggle myToggle;
    public int id;
    Settings settings;

    void Start()
    {
        settings = FindObjectOfType<Settings>();
        settings.showSettings += CheckToggledOnStart;
        myToggle = GetComponent<Toggle>();
    }

    public void CheckToggledOnStart()
    {
        myToggle.isOn = PlayerPrefs.GetInt(Settings.movement) == id;
    }

    public void OnChanged(bool value)
    {
        if (true)
            PlayerPrefs.SetInt(Settings.movement, id);
    }
}
