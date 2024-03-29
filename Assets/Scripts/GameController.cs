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
	[HideInInspector] public List<Wave> waves = new();
	[SerializeField] private ListOfWaves listOfWaves;


	[SerializeField] private int money;
	[SerializeField] private int health;
	[SerializeField] private bool isPlaying;
	public bool IsPlaying => isPlaying;


	// FIXME: Should be put inside a scriptable object.
	[Header("Defaults")]
	[SerializeField] private int waveDefault = 0;
	[SerializeField] private int moneyDefault = 100;
	[SerializeField] private int healthDefault = 150;
	[SerializeField] private int waveMoney = 100;
	[SerializeField] public float sellPriceMultiplier = 0.7f;
	[SerializeField] public string currency = "$";


	[Header("Player Settings")]
	public GameSettings Preferences;

	[Header("Fast Forward")]
	public bool fastForward = false;
	[SerializeField, Range(0f, 10f)] private float fastForwardMultiplier = 4f;

	[HideInInspector] public float FastForwardMultiplier => fastForwardMultiplier;


	[Header("Loading")]
	public string mainMenuScene;
	[Range(0f, 100f)] public float artificialLoadingDelay;



	[Header("Runtime events")]
	public UnityEvent m_OnWinGame;
	public UnityEvent m_OnLoseGame;

	// UI Updating
		// Should this be a event class instead??
		// int1 = current, int2 = max
	public UnityEvent<int, int> m_OnWaveUpdate;
	public UnityEvent<int> m_OnMoneyUpdate;
	public UnityEvent<int> m_OnHealthUpdate;

	// Waves
	public UnityEvent m_OnWaveStart;
	public UnityEvent m_OnWaveStop;
	public UnityEvent m_OnWaveTrigger;
	public UnityEvent<bool> m_OnWaveFastForward;
	public UnityEvent<EnemySO> m_OnEnemyKilled;
	public UnityEvent<EnemySO> m_OnEnemyEscaped;

	// Game State
	public UnityEvent m_OnGameStart;
	public UnityEvent m_OnGamePause;
	public UnityEvent<GameSettings> m_OnPreferencesLoad;

	// Building
	public UnityEvent<int> m_OnPurchase;
	public UnityEvent<TowerSO> m_OnSelectBuildable;
	public UnityEvent<bool> m_PurchaseResult;
	public UnityEvent<bool> m_ToggleBuildMode;


	// Upgrading
	public UnityEvent<Guid> m_UpgradeOnSelect;
	public UnityEvent m_UpgradeOnDeselect;
	public UnityEvent<TowerSelection> m_OnMarkTower;
	public UnityEvent<TowerData> m_OnTowerDataUpdate;

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
		LoadPreferences();

		waves = listOfWaves.waves;

        currentWave = waveDefault;
		money = moneyDefault;
		m_OnMoneyUpdate.Invoke(money);
		health = healthDefault;
		m_OnHealthUpdate.Invoke(health);

		isPlaying = true;


		m_OnEnemyKilled.AddListener(EnemyKilled);
		m_OnEnemyEscaped.AddListener(EnemyEscaped);
		m_PurchaseResult.AddListener(PurchaseResult);
		m_OnWaveUpdate.AddListener(OnWaveUpdate);
		m_OnWaveStop.AddListener(OnWaveFinish);
		m_OnLoseGame.AddListener(() => SetIsPlaying(false));
		m_OnWinGame.AddListener(() => SetIsPlaying(false));
    }

	void InitializeEvents() {
		m_OnWinGame ??= new();
		m_OnLoseGame ??= new();


		m_OnWaveUpdate ??= new();
		m_OnMoneyUpdate ??= new();
		m_OnHealthUpdate ??= new();


		m_OnWaveStart ??= new();
		m_OnWaveStop ??= new();
		m_OnWaveTrigger ??= new();
		m_OnWaveFastForward ??= new();

		m_OnGameStart ??= new();
		m_OnGamePause ??= new();
		m_OnPreferencesLoad ??= new();

		m_OnEnemyKilled ??= new();
		m_OnEnemyEscaped ??= new();

		m_OnSelectBuildable ??= new();
		m_OnPurchase ??= new();
		m_PurchaseResult ??= new();
		m_ToggleBuildMode ??= new();

		m_OnMarkTower ??= new();
		m_OnTowerDataUpdate ??= new();
		m_UpgradeOnSelect ??= new();
		m_UpgradeOnDeselect ??= new();
	}

	void OnWaveUpdate(int current, int max) {
		currentWave = current;
	}

	void OnWaveFinish() {
		if (Preferences.AutoStart == true) {
			m_OnWaveTrigger.Invoke();
		}
	
		int value = waveMoney;

		UpdateMoney(value + money);

		if (currentWave == waves.Count - 1) {
			m_OnWinGame.Invoke();
		}
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

	// FIXME: Add types of purchase
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

	public void Sell(GameObject tower, int sellPrice) {
		UpdateMoney(money + sellPrice);
		Destroy(tower);
	}

	void UpdateMoney(int value) {
		money = value;
		m_OnMoneyUpdate.Invoke(money);
	}

	void UpdateHealth(int value) {
		health = value;
		m_OnHealthUpdate.Invoke(health);

		if (health <= 0) {
			m_OnLoseGame.Invoke();
		}
	}

	public void ToggleFastForward() {
		fastForward = !fastForward;
		m_OnWaveFastForward.Invoke(fastForward);
	}

	public void SetIsPlaying(bool value) {
		isPlaying = value;
	}

	void SavePreferences() {
		PlayerPrefs.SetInt("AutoStart", Preferences.AutoStart ? 1 : 0);
		PlayerPrefs.SetInt("DisableBuildMode", Preferences.DisableBuildModeOnBuild ? 1 : 0);

		PlayerPrefs.Save();
	}

	void LoadPreferences() {
		Preferences = new()
		{
			AutoStart = PlayerPrefs.GetInt("AutoStart", 0) == 1,
			DisableBuildModeOnBuild = PlayerPrefs.GetInt("DisableBuildMode", 0) == 1
		};

		m_OnPreferencesLoad.Invoke(Preferences);
	}


	/// This part was stolen from menu controller
	// FIXME: Should this be in a separate script that can be reused
	public void LoadLevel(string sceneName) {
		SavePreferences();
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

	
	public class TowerSelection {
		public GameObject towerGameObject;
		public TowerSO towerSO;

		public TowerSelection(GameObject _towerGameObject, TowerSO _towerSO) {
			towerGameObject = _towerGameObject;
			towerSO = _towerSO;
		}
	}

	// TODO: Add more stats.
	public class TowerData {
		public int kills;

		// Sell price increases with all upgrades bought
		public int sellPrice;

		public List<TowerUpgradeSO> boughtUpgrades;
	}

	public class GameSettings {
		public bool AutoStart;
		public bool DisableBuildModeOnBuild;

		public GameSettings() {
			AutoStart = false;
			DisableBuildModeOnBuild = false;
		}
	}
}