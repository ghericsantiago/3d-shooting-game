using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour {

	public float timeBeforeSpawn;
	public GameObject player;
	public GameObject[] enemy;
	protected GameManager gm;

	protected float spawnTimer;
	// Use this for initialization
	void Start () {
		gm = FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (gm.gameEnded)
			return;

		if (spawnTimer <= 0) {
			SpawnEnemy ();
			spawnTimer = timeBeforeSpawn;
		} else {
			spawnTimer -= Time.deltaTime;
		}
	}

	protected int level = 1;
	void SpawnEnemy(){
		for (int i = 0; i < level; i++) {
			GameObject enemyObj = Instantiate (enemy[0], transform.position + new Vector3(0,0, Random.Range(-25, 25)), Quaternion.identity) as GameObject;
			enemyObj.GetComponent<EnemyController> ().target = player.transform;
		}
		level++;
	}
}
