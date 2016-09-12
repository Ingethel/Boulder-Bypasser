using UnityEngine;

public static class FacePoint {

	public static Vector3 RotateTowards_Lerp(Transform target, Vector3 point, float speed){
		Vector3 targetDir = point - target.position;
		float step = speed * Time.deltaTime;
		Vector3 newDir = Vector3.RotateTowards(target.forward, targetDir, step, 0.0F);
        target.rotation = Quaternion.LookRotation(newDir);
		return newDir;
	}
}
