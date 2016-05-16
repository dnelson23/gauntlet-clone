using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Components.Generic;

public class EnemySpawner : ControllerBase {

	public GameObject[] enemyPrefabs;
	public float spawnRadius = 2;
	public int minNumberToSpawn = 3;
	public int maxNumberToSpawn = 5;
	public int NumRequiredToSpawn = 2;

	private List<GameObject> myEnemies;

	private bool isVisible = true;

	HitPoints _health;
	protected float maxHP = 100f;
    public float CurrentHP
	{
		get { return _health.curHitPoints; }
		private set { _health.SetMaxHitPoints(value); }
	}

    protected override void Awake()
	{
        _health = this.gameObject.AddComponent<HitPoints>();        //using "_parent" instead of "this" brought up an error 
		CurrentHP = maxHP;
	}


	// Use this for initialization
	void Start () {
		myEnemies = new List<GameObject> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if(GetComponentInChildren<Renderer>().IsVisibleFrom(Camera.main))
        {
            isVisible = true;
        }
        else
        {
            isVisible = false;
        }

		if (isVisible) {
			CheckForMySpawns ();

			if (myEnemies.Count <= NumRequiredToSpawn)
				SpawnEnemies ();
		}
	}

	GameObject RandomEnemyToSpawn()
	{
		return enemyPrefabs [Random.Range (0, enemyPrefabs.Length)];
	}


	//Check if the locations radius is clear to spawn
	bool CheckSpawnPosition(Vector3 targetPos, float radius){
		int colliders = Physics.OverlapSphere (targetPos, radius).Length;
		if (colliders == 0)
			return true;
		else
			return false;
	}

	void SpawnEnemies()
	{
		Vector3 newPos = Vector3.zero;
		int amount = Random.Range (minNumberToSpawn, maxNumberToSpawn);
        int i = 0;
        do
        {
            newPos.x = (Random.insideUnitCircle * spawnRadius).x;
            newPos.z = (Random.insideUnitCircle * spawnRadius).y;
            newPos += this.transform.position;

            if (CheckSpawnPosition(newPos, 0.12f))
            {		//if true then spawn enemy at that pos
                GameObject tEnemy = Instantiate(RandomEnemyToSpawn(), newPos, Quaternion.identity) as GameObject;
                myEnemies.Add(tEnemy);
                i++;
            }
        } while(i < amount);
	}

	void CheckForMySpawns()
	{
		if(myEnemies.Count > 0)
		{
			foreach (var enemy in myEnemies) {
				if (enemy == null)
					myEnemies.Remove (enemy);
			}
		}
	}
}


