﻿using UnityEngine;
using System.Collections;
using Assets.Scripts.Hero;
using Assets.Scripts.Enemy;
using Assets.Scripts.Components.Generic;

public class EnemySorcerer : EnemyBase {

    public float _speed = 0.04f;

    public bool isShowing = true;
    private float visibleFlashTime = 0.75f;
    private float invisibleFlashTime = 0.25f;

    private MeshRenderer[] _renderers;

	protected override void Awake(){
		base.Awake();

        _renderers = GetComponentsInChildren<MeshRenderer>();

	}

	void Start()
	{
        damage = 5f;

		StartCoroutine(FlashSelf(visibleFlashTime, invisibleFlashTime));
	}

	void FixedUpdate()
    {
		if (!GetComponentInChildren<Renderer>().IsVisibleFrom(Camera.main))
			return;

        //2d vector movement not working
		//_movement.SetMoveVector (FindClosestAlivePlayer ().transform.position);


        Vector3 targetLoc = FindClosestAlivePlayer ().gameObject.transform.position;
        this.transform.position = Vector3.MoveTowards(_parent.position, targetLoc, _speed);
        this.transform.LookAt(targetLoc);


		if (CurrentHP <= 0)
			Destroy ();

	}

	void OnCollisionStay(Collision other)
	{
        HeroBase player = other.gameObject.GetComponent<HeroBase>();
		HitPoints playerHP = other.gameObject.GetComponent<HitPoints> ();
		if (player && playerHP)
		{
			playerHP.TakeDamage (damage);
		}
	}
		
    IEnumerator FlashSelf(float visTime, float invisTime){

		if (isShowing)      
        {
			isShowing = false;
            foreach (MeshRenderer renderer in _renderers)
            {
                renderer.enabled = false;
            }

            yield return new WaitForSeconds(invisTime);
        }
        else
        {
			isShowing = true;
            foreach (MeshRenderer renderer in _renderers)
            {
                renderer.enabled = true;
            }

            yield return new WaitForSeconds(visTime);
        }
        StartCoroutine(FlashSelf(visTime, invisTime));
    }
}
