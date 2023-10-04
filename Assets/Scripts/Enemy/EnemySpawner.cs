using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
	// Pathing
	private List<Transform> nodes;


	// Waves
	public List<Wave> waves = new();
	[SerializeField]private int currentWaveIndex = 0;
	private bool isWaveRunning = false;
	private bool isPoolSpawned = false;


	public List<GameObject> enemies = new();
	private int enemiesKilled = 0;
	private int enemiesFinished = 0;

	// Events

	// Wave Events
	[HideInInspector] public UnityEvent WaveStarted;
	[HideInInspector] public UnityEvent WaveStopped;
	
	// Enemy Events
	[HideInInspector] public UnityEvent EnemyKilled;
	[HideInInspector] public UnityEvent EnemyFinished; // Invoked when an enemy reaches end of level.


	void Start()
	{
		nodes = NodeManager.Instance.nodes;

		ResetWave();
		InitializeEvents();

		SpawnEnemyPool(waves[currentWaveIndex]);
		StartWave();
	}

	void InitializeEvents() {
		WaveStarted ??= new();
		WaveStopped ??= new();

		EnemyKilled ??= new();
		EnemyFinished ??= new();

		EnemyKilled.AddListener(CountKilled);
		EnemyFinished.AddListener(CountFinished);
	}

	void CountKilled() {
		enemiesKilled += 1;
	}

	void CountFinished() {
		enemiesFinished += 1;
	}

	void ResetWave() {
		enemies = new List<GameObject>();
		isWaveRunning = false;
		isPoolSpawned = false;
		enemiesKilled = 0;
		enemiesFinished = 0;
	}

	void SpawnEnemyPool(Wave wave)
	{
		if (isPoolSpawned) return;

		foreach (var containers in wave.enemyContainers)
		{
			EnemyObject enemyObject = containers.enemyObject;
			var amount = containers.amount;

			for (int i = 0; i < amount; i++)
			{
				var enemy = Instantiate(enemyObject.enemyPrefab, Vector3.zero, Quaternion.identity);
				enemy.SetActive(false);

				// Translate enemy to start position. ie first node.
				enemy.transform.position = nodes[0].position;

				// Initialize enemy
				var mover = enemy.GetComponent<EnemyMover>();
				var enemyComponent = enemy.GetComponent<Enemy>();
				enemyComponent.m_EnemyObject = enemyObject;
				enemyComponent.m_Mover = mover;
				enemyComponent.m_Spawner = this;
				enemyComponent.Initialize();
				var materials = enemy.GetComponent<MeshRenderer>().materials;
				materials[0] = enemyObject.enemyMaterial;
				enemy.GetComponent<MeshRenderer>().materials = materials;

				enemies.Add(enemy);
			}
		}

		isPoolSpawned = true;
	}

	void StartWave()
	{
		if (isWaveRunning == true) return;
		if (isPoolSpawned == false) return;

		StartCoroutine(SpawnWave());

		isWaveRunning = true;

		WaveStarted.Invoke();
	}

	void StopWave()
	{
		foreach(var enemy in enemies)
		{
			Destroy(enemy);
		}

		ResetWave();
		WaveStopped.Invoke();

		// Temporary

		currentWaveIndex = (currentWaveIndex + 1) % waves.Count;
		SpawnEnemyPool(waves[currentWaveIndex]);
		StartWave();
	}
	void Update()
	{
		if (isWaveRunning == false) return;
		if (isPoolSpawned == false) return;

		if (enemiesFinished + enemiesKilled == enemies.Count) StopWave();
	}

	IEnumerator SpawnWave() {
		foreach (var enemy in enemies) {
			enemy.SetActive(true);

			yield return new WaitForSeconds(waves[currentWaveIndex].timeBetweenEnemySpawn);
		}

		yield break;
	}
}