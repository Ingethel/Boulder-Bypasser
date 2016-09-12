using UnityEngine;
using System;

public class PlayerState : MonoBehaviour {

	public bool alive;

    public event Action playerDeathEvents;
    
	void Start(){
		alive = true;
	}

	void OnCollisionEnter() {
		if (alive) {
			alive = false;
            if (playerDeathEvents != null) {
                playerDeathEvents();
            }
		}
	}

}
