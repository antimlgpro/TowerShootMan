using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class TowerStatistics : MonoBehaviour
{
	public Guid GUID;

	private bool isSelected = false;
	private RadiusSphere radiusSphere;
	private Tower tower;

	[SerializeField] private Stats towerStats;

    // Start is called before the first frame update
    void Start()
    {
		GUID = Guid.NewGuid();
        isSelected = false;
		radiusSphere = GetComponentInChildren<RadiusSphere>();
		tower = GetComponent<Tower>();


		GameController.Instance.m_UpgradeOnSelect.AddListener(OnSelect);
		GameController.Instance.m_UpgradeOnDeselect.AddListener(OnDeselect);

		towerStats = new();
		towerStats.OnStatChange.AddListener(OnStatsChange);
    }

	private void OnStatsChange()
	{
		UpdateUI();
	}

	public Guid GetGUID() {
		return GUID;
	}

	// If target equals this tower guid. we got selected
	void OnSelect(Guid targetGUID) {
		if (targetGUID.Equals(GUID)) {
			isSelected = true;
			radiusSphere.SetActive(true);
			Debug.LogFormat("Successfully selected {0}", GUID.ToString());

			GameController.Instance.m_OnMarkTower.Invoke(new GameController.TowerSelection(gameObject, tower.towerObject));
			UpdateUI();
		} else {
			isSelected = false;
			radiusSphere.SetActive(false);
			Debug.LogFormat("Successfully deselected {0}", GUID.ToString());
		}
	}

	// if this is ran all towers are deselected.
	void OnDeselect() {
		isSelected = false;
		radiusSphere.SetActive(false);
		Debug.LogFormat("Successfully deselected {0}", GUID.ToString());
	}

	void UpdateUI() {
		var towerData = new GameController.TowerData
		{
			kills = towerStats.Kills,
			sellPrice = Mathf.FloorToInt(tower.towerObject.cost * GameController.Instance.sellPriceMultiplier),
			availableUpgrades = towerStats.AvailableUpgrades
		};
		GameController.Instance.m_OnTowerDataUpdate.Invoke(towerData);
	}

	[Serializable]
	private class Stats {
		private int kills;
		private int moneyEarned;
		private int totalMoneySpent;
		private List<TowerUpgradeSO> availableUpgrades;

		//Fields
		public int Kills { get => kills; set {kills = value; OnStatChange.Invoke(); } }
		public int MoneyEarned { get => moneyEarned; set { moneyEarned = value; OnStatChange.Invoke(); } }
		public int TotalMoneySpent { get => totalMoneySpent; set { totalMoneySpent = value; OnStatChange.Invoke(); } }
		public List<TowerUpgradeSO> AvailableUpgrades { get => availableUpgrades; set { availableUpgrades = value; OnStatChange.Invoke(); } }
		public UnityEvent OnStatChange;


		public Stats() {
			OnStatChange ??= new();
		}
	}
}
