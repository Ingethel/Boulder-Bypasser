using UnityEngine;

public static class FollowPoint
{
    
    public static void follow2D(Transform target, Vector3 pos)
    {
        target.position = new Vector3(pos.x, pos.y, target.position.z);
    }

    public static void follow2D_Lerp(Transform target, Vector3 pos, float speed)
    {
        target.position = Vector2.Lerp(target.position, pos, speed);
    }

    public static void follow3D(Transform target, Vector3 pos)
    {
        target.position = pos;
    }

    public static void follow3D_Lerp(Transform target, Vector3 pos, float speed)
    {
        target.position = Vector3.Lerp(target.position, pos, speed);
    }

}
