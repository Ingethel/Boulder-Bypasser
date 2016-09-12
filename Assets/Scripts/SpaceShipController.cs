using UnityEngine;

public class SpaceShipController : MonoBehaviour {

    PlayerState player;
    public GameObject explosion;

    void Start () {
        player = FindObjectOfType<PlayerState>();
        player.playerDeathEvents += OnPlayerDeath;
    }

    public void OnPlayerDeath() {
        player.playerDeathEvents -= OnPlayerDeath;
        Instantiate(explosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
    }
}
