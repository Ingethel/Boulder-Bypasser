using UnityEngine;

public class GeneratorScipt : MonoBehaviour {

    public ParticleSystem beam;
    PlayerState player;

    public void Start() {
        beamState(true);
        bodyState(false);
        player = FindObjectOfType<PlayerState>();
        player.playerDeathEvents += OnPlayerDeath;
    }

    public void bodyState(bool f) {
        if (f) {
            gameObject.AddComponent<Rigidbody>();
        }
        else {
            if (GetComponent<Rigidbody>())
                Destroy(GetComponent<Rigidbody>());
        }
    }

    public void beamState(bool f) {
        var emission = beam.emission;
        emission.enabled = f;
    }

    public void OnPlayerDeath() {
        beamState(false);
        bodyState(true);
        player.playerDeathEvents -= OnPlayerDeath;
    }
}
