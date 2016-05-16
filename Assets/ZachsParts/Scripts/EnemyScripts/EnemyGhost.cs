using UnityEngine;
using System.Collections;
using Assets.Scripts.Enemy;
using Assets.Scripts.Components.Generic;

public class EnemyGhost : EnemyBase {

	protected VectorMovement2D _movementRef;
	public float _speed = 100;



	void Start(){
		_movementRef = this.gameObject.GetComponent<VectorMovement2D> ();

        damage = 3f;
	}

	void Update(){

		if (isVisible) {
			Vector3 targetLoc = FindClosestAlivePlayer ().gameObject.transform.position;
            this.transform.position = Vector3.MoveTowards(_parent.position, targetLoc, _speed);
			this.transform.LookAt(targetLoc);

            //_movementRef.SetMoveVector (FindClosestAlivePlayer ().transform.position);

		}
	}


}
