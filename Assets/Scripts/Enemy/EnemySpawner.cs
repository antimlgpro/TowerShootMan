using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	// Pathing
	private List<Transform> nodes;


	// Waves
	public List<Wave> waves = new();
	private int currentWaveIndex = 0;
	private bool isWaveRunning = false;
	private bool isWaveSpawned = false;


	public List<GameObject> enemies = new();
	private bool enemiesVisible = false;

	void Start()
	{
		nodes = NodeManager.Instance.nodes;

		SpawnWave(waves[currentWaveIndex]);
		StartWave();
	}

	void SpawnWave(Wave wave)
	{
		if (isWaveSpawned) return;

		foreach (var containers in wave.enemyContainers)
		{
			var enemyObject = containers.enemyObject;
			var amount = containers.amount;

			for (int i = 0; i < amount; i++)
			{
				var enemy = Instantiate(enemyObject.enemyPrefab, Vector3.zero, Quaternion.identity);
				enemy.SetActive(false);

				enemy.transform.position = nodes[0].position;

				// Initialize enemy
				var mover = enemy.GetComponent<EnemyMover>();
				mover.Initialize(enemyObject, containers.speedMultiplier);
				enemy.GetComponent<Enemy>().SetEnemyObject(enemyObject);

				enemies.Add(enemy);
			}
		}

		isWaveSpawned = true;
	}

	void StartWave()
	{
		if (isWaveRunning == true) return;

		if (isWaveSpawned == false) return;

		isWaveRunning = true;
	}

	void StopWave()
	{
		for (int i = 0; i < enemies.Count; i++)
		{
			var enemy = enemies[i];
			enemies.RemoveAt(i);
			Destroy(enemy);
		}

		enemies = new List<GameObject>();

		isWaveRunning = false;
		isWaveSpawned = false;
		enemiesVisible = false;
		enemyIndex = 0;
		timeElapsed = 0;

		SpawnWave(waves[1]);
		StartWave();
	}

	float timeElapsed = 0;
	int enemyIndex = 0;

	// Update is called once per frame
	void Update()
	{
		if (isWaveRunning == false) return;
		if (isWaveSpawned == false) return;


		if (enemies.Count <= 1) StopWave();


		// Will this be laggy when there is a lot of enemies
		for (int i = 0; i < enemies.Count; i++)
		{
			var enemy = enemies[i];
			if (enemy.GetComponent<EnemyMover>().StopMoving)
			{
				Destroy(enemy);
				enemies.RemoveAt(i);
			}
		}

		if (!enemiesVisible)
		{
			timeElapsed += Time.deltaTime;
			int enemiesActivated = 0;
			for (int i = 0; i < enemies.Count; i++)
			{
				var enemy = enemies[i];

				enemyIndex = Mathf.RoundToInt(timeElapsed / 0.25f);

				if (enemyIndex < i)
				{
					continue;
				}

				enemy.SetActive(true);
				enemiesActivated += 1;
			}

			if (enemiesActivated == enemies.Count) enemiesVisible = true;
		}
	}
}