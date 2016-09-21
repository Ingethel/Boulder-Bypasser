using UnityEngine;

public class Pillar : IPoolObject
{

    Transform[] childTransforms;
    Vector3[] localPoss;
    bool hasChildren;

    void Start()
    {
        hasChildren = transform.childCount > 0;
        if (hasChildren)
        {
            childTransforms = GetComponentsInChildren<Transform>();
            localPoss = new Vector3[childTransforms.Length];
            for (int i = 0; i < childTransforms.Length; i++)
            {
                localPoss[i] = childTransforms[i].localPosition;
            }
        }

    }

    public override void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        base.Spawn(position, rotation, scale);
        if (hasChildren)
        {
            for (int i = 0; i < childTransforms.Length; i++)
            {
                childTransforms[i].localPosition = localPoss[i];
            }
        }
        ready = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        ready = true;
    }

}
