using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{

    public Transform target;

    public float movementSpeed;
    public event Action movementIncreasedEvents;

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
        cameraFollow = FindObjectOfType<CameraFollow>();
        mainEngineBeam = mainEngine.sizeOverLifetime;
        Physics.gravity = new Vector3(0, 0, -movementSpeed);
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
        float scalar = mainEngineBeam.size.curveMultiplier;
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
        float scalar = mainEngineBeam.size.curveMultiplier;
        while (scalar > 1)
        {
            scalar -= 0.1f;
            boostSpeed -= 0.1f;
            mainEngineBeam.size = new ParticleSystem.MinMaxCurve(scalar, curve);
            yield return new WaitForEndOfFrame();
        }
        boostCoolingEnabled = false;
    }
    
    public void MoveIncrease(float value)
    {
        movementSpeed += value;

        Physics.gravity = new Vector3(0, 0, - movementSpeed);

        if (movementIncreasedEvents != null)
            movementIncreasedEvents();
    }
}
