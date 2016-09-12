using UnityEngine;

public class IPoolObject : MonoBehaviour
{

    public bool ready { get; protected set; }

    protected virtual void Awake()
    {
        ready = true;
    }

    public virtual void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
        gameObject.SetActive(true);
    }

    public virtual void Destroy()
    {
        gameObject.SetActive(false);
    }
}

