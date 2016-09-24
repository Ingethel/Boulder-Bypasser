using UnityEngine;

public class SpaceShipController : MonoBehaviour {

    PlayerState player;
    public GameObject explosion;

    Color materialColor;
    MeshRenderer _renderer;

    GameManager manager;

    void Start () {
        player = FindObjectOfType<PlayerState>();
        player.playerDeathEvents += OnPlayerDeath;
        _renderer = GetComponent<MeshRenderer>();
        materialColor = _renderer.material.color;
        manager = FindObjectOfType<GameManager>();
        manager.InGameEvent += SetTransparency;
        manager.InGameEvent += EnableRenderer;
    }

    public void OnPlayerDeath() {
        player.playerDeathEvents -= OnPlayerDeath;
        Instantiate(explosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }

    public void SetTransparency()
    {
        materialColor.a = 1 - PlayerPrefs.GetFloat(Settings.getString(Settings.LABELS.CHARACTER_TRANSPARENCY));
        _renderer.material.color = materialColor;
    }

    public void EnableRenderer()
    {
        bool enable = PlayerPrefs.GetInt(Settings.getString(Settings.LABELS.CAMERA_SETTINGS)) == 1 ? true : false;
        _renderer.enabled = enable;
    }

}
