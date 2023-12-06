using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
	// Pathing
	private List<Transform> nodes;


	// Waves
	private List<Wave> waves;
	[SerializeField] private int currentWaveIndex = 0;
	private bool isWaveRunning = false;
	private bool isPoolSpawned = false;


	public List<GameObject> enemies = new();
	private int enemiesKilled = 0;
	private int enemiesFinished = 0;

	// Events

	// Wave Events
	[HideInInspector] private UnityEvent OnWaveTrigger; // TODO: Implement starting waves with this trigger.
	[HideInInspector] private UnityEvent OnWaveStart;
	[HideInInspector] private UnityEvent OnWaveStop;
	
	// Enemy Events
	[HideInInspector] public UnityEvent<EnemySO> OnEnemyKilled;
	[HideInInspector] public UnityEvent<EnemySO> OnEnemyFinished; // Invoked when an enemy reaches end of level.


	[SerializeField] private bool DEBUG;

	void Start()
	{
		nodes = NodeManager.Instance.nodes;
		waves = GameController.Instance.waves;

		ResetWave();
		InitializeEvents();

		GameController.Instance.m_OnWaveUpdate.Invoke(currentWaveIndex, waves.Count);
	}

	void TriggerWave() {
		if (DEBUG) Debug.LogFormat("Spawning wave {0}", currentWaveIndex);

		SpawnEnemyPool(waves[currentWaveIndex]);
		StartWave();
	}

	void IncrementWave() {
		currentWaveIndex = (currentWaveIndex + 1) % waves.Count;

		GameController.Instance.m_OnWaveUpdate.Invoke(currentWaveIndex, waves.Count);

		if (currentWaveIndex == waves.Count) {
			// We are finished.
		}
	}

	void InitializeEvents() {
		OnWaveStart = GameController.Instance.m_OnWaveStart;
		OnWaveStop = GameController.Instance.m_OnWaveStop;
		OnWaveTrigger = GameController.Instance.m_OnWaveTrigger;

		OnEnemyKilled = GameController.Instance.m_OnEnemyKilled;
		OnEnemyFinished = GameController.Instance.m_OnEnemyEscaped;

		OnEnemyKilled.AddListener(CountKilled);
		OnEnemyFinished.AddListener(CountFinished);
		OnWaveTrigger.AddListener(TriggerWave);
	}

	void CountKilled(EnemySO _) {
		enemiesKilled += 1;
	}

	void CountFinished(EnemySO _) {
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
			EnemySO enemyObject = containers.enemyObject;
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

		GameController.Instance.m_OnWaveUpdate.Invoke(currentWaveIndex, waves.Count);

		StartCoroutine(SpawnWave());

		isWaveRunning = true;

		OnWaveStart.Invoke();
	}

	void StopWave()
	{
		foreach(var enemy in enemies)
		{
			Destroy(enemy);
		}

		ResetWave();
		OnWaveStop.Invoke();

		IncrementWave();
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