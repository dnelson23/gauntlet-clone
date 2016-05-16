using UnityEngine;
using System.Collections;
using Assets.Scripts.Hero;

public class CameraFollow : MonoBehaviour {

	private HeroBase[] playersAlive;

	// Use this for initialization
	void Start () {
		playersAlive = GetAlivePlayers ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newPos;
		newPos = AveragePlayerPosition (true);
		this.transform.position = newPos;
	}

	public HeroBase[] GetAlivePlayers()
	{
		return FindObjectsOfType<HeroBase> ();
	}

	Vector3 AveragePlayerPosition(bool staticZ)
	{
		Vector3 accumulator = Vector3.zero;
		foreach (HeroBase player in playersAlive) {
			accumulator += player.gameObject.transform.position;
		}
		accumulator = accumulator / playersAlive.Length;
		if (staticZ){
			accumulator.z = this.transform.position.z;
		}
		return accumulator;
	}



}
