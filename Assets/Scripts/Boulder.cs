using UnityEngine;

public class Boulder : IPoolObject
{
    Rigidbody rg = null;
    
    public override void Spawn(Vector3 position, Quaternion rotation, Vector3 scale)
    {
        base.Spawn(position, rotation, scale);
        if (rg == null)
            rg = GetComponent<Rigidbody>();
        rg.AddForce(Physics.gravity * scale.x, ForceMode.Impulse);
        rg.AddTorque(transform.eulerAngles * 2, ForceMode.Impulse);

        ready = false;
    }

    public override void Destroy()
    {
        rg.velocity = Vector3.zero;
        base.Destroy();
        ready = true;
    }
    
}