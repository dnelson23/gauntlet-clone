using UnityEngine;
using System.Collections;
using Assets.Scripts.Enemy;
using Assets.Scripts.Components.Generic;

public class EnemyGhost : EnemyBase {

	public float _speed = 0.03f;
    
	void Start()
    {
        damage = 3f;
	}

	void FixedUpdate(){

		if (GetComponentInChildren<Renderer>().IsVisibleFrom(Camera.main)) {
			Vector3 targetLoc = FindClosestAlivePlayer ().gameObject.transform.position;
            this.transform.position = Vector3.MoveTowards(_parent.position, targetLoc, _speed);
			this.transform.LookAt(targetLoc);
		}
	}


}
