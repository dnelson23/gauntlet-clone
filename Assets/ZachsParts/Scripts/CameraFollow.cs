using UnityEngine;
using System.Collections;
using Assets.Scripts.Hero;

public class CameraFollow : MonoBehaviour {

	private HeroBase[] playersAlive;

	// Use this for initialization
	public void InitCamPlayers () {
		playersAlive = GetAlivePlayers ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
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
            accumulator = new Vector3(accumulator.x + player.transform.position.x, 0f, accumulator.z + player.transform.position.z);
			//accumulator += player.gameObject.transform.position;
		}
		accumulator = new Vector3(accumulator.x/playersAlive.Length, 0f, accumulator.z/playersAlive.Length);
		if (staticZ){
			accumulator.y = this.transform.position.y;
		}

        Debug.Log(accumulator);
		return accumulator;
	}



}
