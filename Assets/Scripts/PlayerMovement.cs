using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    public Transform target;
    public float movementSpeed;
    Vector3 rotation;
    Rigidbody rg;
    Vector3 velocity;
    CameraFollow cameraFollow;
    public ParticleSystem mainEngine;
    ParticleSystem.SizeOverLifetimeModule mainEngineBeam;

    float boostSpeed = 1;
    bool boostCoolingEnabled = false;

    public void Start()
    {
        rg = GetComponent<Rigidbody>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        mainEngineBeam = mainEngine.sizeOverLifetime;
    }

    public void UpdateFocus()
    {
        rotation = FacePoint.RotateTowards_Lerp(transform, target.position, 2f);
        velocity = new Vector3(rotation.x, rotation.y, 0) * movementSpeed;
    }

    public void FixedUpdate()
    {
        rg.MovePosition(rg.position + velocity * Time.deltaTime * boostSpeed);
        cameraFollow.AddTrail(transform.position);
    }

    public void SpeedBoost()
    {
        AnimationCurve curve = mainEngineBeam.size.curve;
        float scalar = mainEngineBeam.size.curveScalar;
        if (scalar < 2)
        {
            scalar += 0.1f;
            boostSpeed += 0.1f;
            mainEngineBeam.size = new ParticleSystem.MinMaxCurve(scalar, curve);
        }
    }

    public void BoostCoolDown() {
        if (!boostCoolingEnabled)
            StartCoroutine(CalmBoost());
    }

    IEnumerator CalmBoost()
    {
        boostCoolingEnabled = true;
        AnimationCurve curve = mainEngineBeam.size.curve;
        float scalar = mainEngineBeam.size.curveScalar;
        while (scalar > 1)
        {
            scalar -= 0.1f;
            boostSpeed -= 0.1f;
            mainEngineBeam.size = new ParticleSystem.MinMaxCurve(scalar, curve);
            yield return new WaitForEndOfFrame();
        }
        boostCoolingEnabled = false;
    }

}
