using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIManager : UIManagerBase
{
	[Header("Upgrade UI")]
	private GameObject selectedObject;
	private TowerSO selectedTowerSO;
	private int sellPrice;

	[Header("References")]
	[SerializeField] private TextMeshProUGUI titleText;
	[SerializeField] private TextMeshProUGUI killsText;
	[SerializeField] private TextMeshProUGUI sellPriceText;
	[SerializeField] private Image portraitImage;
	[SerializeField] private Button sellButton;
	[SerializeField] private UpgradeManager upgradeManager;


	[Header("Animation")]
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private float timeToMove;
	[SerializeField] private Vector2 inactivePosition;
	[SerializeField] private Vector2 activePosition;
	private bool isAnimating = false;
	private bool goingOut = false;
	private bool isOpen = false;

	private Vector2 lastPosition;
	private Vector2 target;
	private float elapsedTime;

	public override bool LoadUIManager()
	{
		// Events
		GameController.Instance.m_OnMarkTower.AddListener(SelectObject);
		GameController.Instance.m_OnTowerDataUpdate.AddListener(UpdateUI);

		GameController.Instance.m_UpgradeOnSelect.AddListener(OnSelect);
		GameController.Instance.m_UpgradeOnDeselect.AddListener(OnDeselect);

		// Animation
		lastPosition = inactivePosition;
		target = activePosition;
		StartCoroutine(ToggleAnimation());

		// HACK: This is stuped, fix in ui instead.
		isAnimating = true;
		SwapAnimationTarget();
		elapsedTime = 10f;

		return true;
	}

	void Awake() {
		sellButton.onClick.AddListener(OnSell);
	}

	private void OnDeselect()
	{
		Toggle(false);
	}

	private void OnSelect(Guid _)
	{
		Toggle(true);
	}

	public override void Toggle(bool value)
	{
		//base.Toggle(value);
		if (isOpen == value) return;

		isOpen = value;

		isAnimating = true;
		SwapAnimationTarget();
	}

	private void SelectObject(GameController.TowerSelection towerSelection) {
		if (towerSelection == null) {
			// TODO: Implement default values maybe.
			Toggle(false);
			return;
		} else {
			Toggle(true);
		}

		selectedObject = towerSelection.towerGameObject;
		selectedTowerSO = towerSelection.towerSO;

		titleText.text = selectedTowerSO.name;
		portraitImage.sprite = selectedTowerSO.sprite;
	}

	private void UpdateUI(GameController.TowerData towerData) {
		if (selectedObject == null) return;

		killsText.text = towerData.kills.ToString();
		sellPriceText.text = string.Format("{0}{1}", 
			GameController.Instance.currency, 
			towerData.sellPrice
		);

		sellPrice = towerData.sellPrice;

		upgradeManager.UpdateUI(selectedObject, towerData);
	}

	private void OnSell() {
		GameController.Instance.Sell(selectedObject, sellPrice);

		selectedObject = null;
		selectedTowerSO = null;
		sellPrice = 0;

		Toggle(false);
	}

	void SwapAnimationTarget() {
		Vector2 _target = target;
		target = lastPosition;
		lastPosition = _target;

		elapsedTime = 0f;

		goingOut = target == activePosition;
	}

	IEnumerator ToggleAnimation() {
		while (true) {
			if (Mathf.Approximately(Vector2.Distance(rectTransform.anchoredPosition, target), 0f)) {
				elapsedTime = 0f;
				isAnimating = false;
      			yield return null;
			}

			if (isAnimating) {
				float t = InOut(elapsedTime / timeToMove);

				if (goingOut) {
					t = 1 - OutBounce(1 - t);
				} else {
					t = InOut(t);
				}

				rectTransform.anchoredPosition = Vector2.Lerp(lastPosition, target, t);
				elapsedTime += Time.deltaTime;
      			yield return new WaitForEndOfFrame();
			}
		}
	}

	public static float OutBounce(float t)
	{
		float s = 4f;//1.70158f;
		return t * t * ((s + 1) * t - s);
	}

	private float InOut(float t) {
		return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
	}
}
