using UnityEngine;

public class RotationAroundAxis : MonoBehaviour
{
    public float speed;
    public Vector3 axis;

	void Update () {
        transform.Rotate(axis, speed);
	}

}
