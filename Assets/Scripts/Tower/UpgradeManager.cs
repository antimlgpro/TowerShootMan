using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeManager : MonoBehaviour {
	private GameObject currentObject;
	private List<UpgradeObject> upgradeObjects;

    void Start() {
        upgradeObjects = new();
		foreach (Transform child in transform) {
			var upgradeObject = child.GetComponent<UpgradeObject>();
			upgradeObjects.Add(upgradeObject);
			upgradeObject.OnBoughtUpgrade.AddListener(OnBuyUpgrade);
		}
    }

	public void UpdateUI(GameObject currentTowerObject, GameController.TowerData towerData) {
		if (currentObject == currentTowerObject) return;
		currentObject = currentTowerObject;

		foreach (var upgradeObject in upgradeObjects) {
			upgradeObject.Initialize(towerData.boughtUpgrades.Where(x => x.type == upgradeObject.upgradeType).ToList());
		}
	}

	void OnBuyUpgrade(TowerUpgradeSO towerUpgradeSO) {
		currentObject.GetComponent<Tower>().AddUpgrade(towerUpgradeSO);
		TowerStatistics statistics = currentObject.GetComponent<TowerStatistics>();
		statistics.AddUpgrade(towerUpgradeSO);
	}
}
