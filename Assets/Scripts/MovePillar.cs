using UnityEngine;

public class MovePillar : MonoBehaviour
{

    CaveManager caveManager;
    public Transform deathArea;
    public Transform pivot;
    Boulder boulderScript;
    public float speed;

    void Start()
    {
        caveManager = FindObjectOfType<CaveManager>();
        boulderScript = GetComponent<Boulder>();
    }

    void FixedUpdate()
    {
        float relativeSpeed = caveManager.speed * speed;
        transform.position -= Time.deltaTime * Vector3.forward * relativeSpeed;
        if (boulderScript != null)
            if (Mathf.Abs(pivot.position.z - deathArea.position.z) < 1)
            {
                boulderScript.Destroy();
            }
    }

}
