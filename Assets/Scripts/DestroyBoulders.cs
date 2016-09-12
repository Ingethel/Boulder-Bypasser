using UnityEngine;

public class DestroyBoulders : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		GameObject obj = other.gameObject;
		if (obj.GetComponent<IPoolObject> ()) {
			obj.GetComponent<IPoolObject> ().Destroy ();
		} 
	}

}
