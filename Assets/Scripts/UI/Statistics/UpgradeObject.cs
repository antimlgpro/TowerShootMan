using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeObject : MonoBehaviour
{
	// Upgrades for this category
	[SerializeField] private List<TowerUpgradeSO> towerUpgrades;
	[SerializeField] private string maxUpgradeText;
	[SerializeField] private int maxUpgrades; // Based on dots image, but this allows for easy changing that.
	

	[Header("References")]
	[SerializeField] private Image dotsFill;
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private Button upgradeButton;
	[SerializeField] private TextMeshProUGUI priceText;

	private int currentUpgradeIndex = 0;
	private int upgradesBought = 0; 

	private string currency;

	void Start() {
		dotsFill.fillAmount = 0f;
		currency = GameController.Instance.currency;

		upgradeButton.onClick.AddListener(OnClickPurchase);

		// Load the first upgrade into UI
		LoadUpgrade(0);
	}

	void OnClickPurchase() {
		BuyUpgrade(currentUpgradeIndex);
	}
	
	void OnValidate() {
		if (towerUpgrades.Count > 3) {
			Debug.LogError("Max three upgrades at once");
		}
	}

	void BuyUpgrade(int index) {
		if (index > Mathf.Min(towerUpgrades.Count, maxUpgrades)) return;
		if (index >= towerUpgrades.Count) return;

		TowerUpgradeSO towerUpgrade = towerUpgrades[index];

		int price = towerUpgrade.cost;

		if (GameController.Instance.Purchase(price) == false) {
			// We could not purchase. Show error or some kind of animation.
			return;
		}

		// Send event to affected tower
		// Tower.onPurchase.Invoke(towerUpgrade);

		LoadUpgrade(index + 1);
	}

	void LoadUpgrade(int index) {
		if (index >= Mathf.Min(towerUpgrades.Count, maxUpgrades)) {
			UpdateDots(index);
			titleText.text = maxUpgradeText;
			priceText.text = "";
			upgradeButton.interactable = false;

			return;
		}
		if (index >= towerUpgrades.Count) return;

		TowerUpgradeSO towerUpgrade = towerUpgrades[index];

		UpdateDots(index);
		titleText.text = towerUpgrade.upgradeName;
		priceText.text = string.Format("{0}{1}", currency, towerUpgrade.cost.ToString());

		currentUpgradeIndex = index;
	}

	void UpdateDots(int amount) {
		if (amount > 3) return;
		if (amount < 0) return;

		dotsFill.fillAmount = 0.333f * amount;
	}
}