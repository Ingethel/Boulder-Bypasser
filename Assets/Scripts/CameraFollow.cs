using UnityEngine;
using System.Collections.Generic;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float distance;
    Vector3 offset;
    GameManager manager;
    Queue<Vector3> trail;
    Vector3 head = Vector3.zero;

    void Start()
    {
        trail = new Queue<Vector3>();
        offset = target.forward * distance;
        manager = FindObjectOfType<GameManager>();
    }

    void FixedUpdate()
    {
        if (manager.currentState == GameManager.GameState.INGAME)
        {
            FollowPoint.follow3D_Lerp(transform, head - offset, 0.1f);
            FacePoint.RotateTowards_Lerp(transform, target.position, 0.5f);
        }
    }

    public void AddTrail(Vector3 pos) {
        trail.Enqueue(pos);
        if (trail.Count > 10)
            head = trail.Dequeue();
    }

}
