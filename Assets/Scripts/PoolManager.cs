using UnityEngine;
using System.Collections.Generic;

public class PoolManager : MonoBehaviour {

	Dictionary<int, Queue<PoolObject>> pool = new Dictionary<int, Queue<PoolObject>> ();
	
	static PoolManager _instance;
	public static PoolManager instance{
		get{
			if(_instance == null){
				_instance = FindObjectOfType<PoolManager>();
			}
			return _instance;
		}
	}

	public void CreatePool(GameObject obj, int size){
		int poolkey = obj.GetInstanceID ();
		if (!pool.ContainsKey (poolkey)) {
			pool.Add(poolkey, new Queue<PoolObject>());
			GameObject o = new GameObject(obj.name);
			o.transform.parent = transform;
			
			for(int i = 0; i < size; i++){
				PoolObject newBoulder = new PoolObject(Instantiate (obj) as GameObject);
				pool[poolkey].Enqueue(newBoulder);
				newBoulder.setParent(o.transform);
			}
		}
	}

	public void CreateMultiPool(GameObject[] obj, float[] probs, int size, int key, string name){
		if (obj.Length == probs.Length) {
			if (!pool.ContainsKey (key)) {
				pool.Add(key, new Queue<PoolObject>());
				GameObject o = new GameObject(name);
				o.transform.parent = transform;

				for(int i = 0; i < size; i++){
					float value = Random.value;
					int index = 0;
					float prob = probs[index];
					while(value > prob && index < obj.Length-1){
						index ++;
						prob += probs[index];
					}
					PoolObject newBoulder = new PoolObject(Instantiate (obj[index]) as GameObject);
					pool[key].Enqueue(newBoulder);
					newBoulder.setParent(o.transform);
				}

			}
		} else {
			Debug.Log("Error Creating Multipool");
		}

	}

	public void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation, Vector3 scale) {
		int poolKey = prefab.GetInstanceID ();
		
		if (pool.ContainsKey (poolKey)) {
			PoolObject objectToReuse = pool [poolKey].Dequeue ();
			if(objectToReuse.poolObjectScript.ready){
				objectToReuse.Spawn (position, rotation, scale);
			}
			pool [poolKey].Enqueue (objectToReuse);
		}
	}

	public void SpawnObject(int key, Vector3 position, Quaternion rotation, Vector3 scale) {
		if (pool.ContainsKey (key)) {
			PoolObject objectToReuse = pool [key].Dequeue ();
			if(objectToReuse.poolObjectScript.ready){
				objectToReuse.Spawn (position, rotation, scale);
			}
			pool [key].Enqueue (objectToReuse);
		}
	}
	
	public void SpawnObject(GameObject prefab, Vector3 position, Quaternion rotation){
		SpawnObject (prefab, position, rotation, Vector3.one);
	}
	public void SpawnObject(GameObject prefab, Vector3 position, Vector3 scale){
		SpawnObject (prefab, position, Quaternion.identity, scale);
	}
	public void SpawnObject(GameObject prefab, Vector3 position){
		SpawnObject (prefab, position, Quaternion.identity, Vector3.one);
	}
}


public class PoolObject {
	GameObject _object;
	Transform _transform;

	bool hasPoolObjectComponent;
	public IPoolObject poolObjectScript;

	public PoolObject(GameObject obj){
		_object = obj;
		_transform = _object.GetComponent<Transform> ();

		if (_object.GetComponent<IPoolObject>()) {
			hasPoolObjectComponent = true;
			poolObjectScript = _object.GetComponent<IPoolObject>();
		}
		_object.SetActive (false);

	}
	
	public void Spawn(Vector3 position, Quaternion rotation, Vector3 scale){
		if (hasPoolObjectComponent) {
			poolObjectScript.Spawn (position, rotation, scale);
		}
	}

	public void Destroy(){
		if (hasPoolObjectComponent) {
			poolObjectScript.Destroy ();
		}
	}

	public void setParent(Transform t){
		_transform.parent = t;
	}
}