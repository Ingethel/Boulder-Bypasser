using UnityEngine;

public class ICollectible : IPoolObject
{
    
    protected void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("Player"))
        {
            Behave();
            Destroy();
        }
    }

    public override void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        base.Spawn(position, rotation, scale);
        ready = false;
    }

    public override void Destroy()
    {
        base.Destroy();
        ready = true;
    }

    protected virtual void Behave() {}

}
