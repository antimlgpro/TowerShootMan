using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


// This class controlls UI, wave spawner and player state. 
public class GameController : MonoBehaviour
{
	[SerializeField] private int currentWave;
	[SerializeField] public List<Wave> waves = new();


	[SerializeField] private int money;
	[SerializeField] private int health;

	// FIXME: Should be put inside a scriptable object.
	[Header("Defaults")]
	[SerializeField] private int waveDefault = 0;
	[SerializeField] private int moneyDefault = 100;
	[SerializeField] private int healthDefault = 150;
	[SerializeField] private int waveMoney = 100;


	[Header("Loading")]
	public string mainMenuScene;
	[Range(0f, 100f)] public float artificialLoadingDelay;



	[Header("Runtime events")]
	// UI Updating
		// Should this be a event class instead??
		// int1 = current, int2 = max
	[SerializeField] public UnityEvent<int, int> m_OnWaveUpdate;
	[SerializeField] public UnityEvent<int> m_OnMoneyUpdate;
	[SerializeField] public UnityEvent<int> m_OnHealthUpdate;

	// Waves
	[SerializeField] public UnityEvent m_OnWaveStart;
	[SerializeField] public UnityEvent m_OnWaveStop;
	[SerializeField] public UnityEvent m_OnWaveTrigger;
	[SerializeField] public UnityEvent<EnemySO> m_OnEnemyKilled;
	[SerializeField] public UnityEvent<EnemySO> m_OnEnemyEscaped;


	// Game State
	[SerializeField] public UnityEvent m_OnGameStart;
	[SerializeField] public UnityEvent m_OnGamePause;

	// Building
	[SerializeField] public UnityEvent<TowerSO> m_OnSelectBuildable;
	[SerializeField] public UnityEvent<int> m_OnPurchase;
	[SerializeField] public UnityEvent<bool> m_PurchaseResult;
	[SerializeField] public UnityEvent<bool> m_ToggleBuildMode;

	// I really do not want to use a singelton here.
	// Singelton pattern stolen from https://gamedevbeginner.com/singletons-in-unity-the-right-way/
	public static GameController Instance { get; private set; }
	private void Awake()
	{
		// If there is an instance, and it's not me, delete myself.
		if (Instance != null && Instance != this)
		{
			Destroy(this);
		}
		else
		{
			Instance = this;
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		InitializeEvents();

        currentWave = waveDefault;
		money = moneyDefault;
		m_OnMoneyUpdate.Invoke(money);
		health = healthDefault;
		m_OnHealthUpdate.Invoke(health);


		m_OnEnemyKilled.AddListener(EnemyKilled);
		m_OnEnemyEscaped.AddListener(EnemyEscaped);
		m_PurchaseResult.AddListener(PurchaseResult);
		m_OnWaveUpdate.AddListener(OnWaveUpdate);
		m_OnWaveStop.AddListener(OnWaveFinish);
    }

	void InitializeEvents() {
		m_OnWaveUpdate ??= new();
		m_OnMoneyUpdate ??= new();
		m_OnHealthUpdate ??= new();


		m_OnWaveStart ??= new();
		m_OnWaveStop ??= new();
		m_OnWaveTrigger ??= new();

		m_OnGameStart ??= new();
		m_OnGamePause ??= new();

		m_OnEnemyKilled ??= new();
		m_OnEnemyEscaped ??= new();

		m_OnSelectBuildable ??= new();
		m_OnPurchase ??= new();
		m_PurchaseResult ??= new();
		m_ToggleBuildMode ??= new();
	}

	void OnWaveUpdate(int current, int max) {
		currentWave = current;
	}

	void OnWaveFinish() {
		int value = waveMoney;

		UpdateMoney(value + money);
	}

	void PurchaseResult(bool result) {
		if (result)
			print("Successfully purchased");
		else
			print("Failed to purchase");
	}

	// When an enemy is killed money is increased by the enemy value.
	void EnemyKilled(EnemySO enemyObject) {
		UpdateMoney(money + enemyObject.moneyValue);
	}

	// When an enemy escapes health is decreased by the enemy escape penalty.
	void EnemyEscaped(EnemySO enemyObject) {
		UpdateHealth(health - enemyObject.escapePenalty);
	}

	public bool CanAfford(int amount) {
		return money >= amount;
	}

	public bool PurchaseTower(TowerSO towerObject) {
		return Purchase(towerObject.cost);
	}

	public bool Purchase(int cost) {
		bool result = InternalPurchase(cost);
		m_PurchaseResult.Invoke(result);

		return result;
	}

	bool InternalPurchase(int cost) {
		// Do we have enough money for this purchase?
		if ((money - cost) >= 0) {
			UpdateMoney(money - cost);
			return true;
		} else {
			return false;
		}
	}

	void UpdateMoney(int value) {
		money = value;
		m_OnMoneyUpdate.Invoke(money);
	}

	void UpdateHealth(int value) {
		health = value;
		m_OnHealthUpdate.Invoke(health);
	}


	/// This part was stolen from menu controller
	// FIXME: Should this be in a separate script that can be reused
	public void LoadLevel(string sceneName) {
		LoadingScreen.Instance.m_OnLoadLevel.Invoke();
		StartCoroutine(LoadLevelAsync(sceneName));
	}

	IEnumerator LoadLevelAsync(string sceneToLoad) {
		yield return new WaitForSeconds(artificialLoadingDelay);

		AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

		operation.completed += (_) => {
			LoadingScreen.Instance.m_OnFadeLoadingScreen.Invoke();
		};

		while (!operation.isDone)
        {
            yield return null;
        }
	}
}
