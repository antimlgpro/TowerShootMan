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

	[Header("References")]
	private TextMeshProUGUI titleText;
	private TextMeshProUGUI killsText;
	private TextMeshProUGUI sellPriceText;
	private Image portraitImage;

	//private UpgradeManager upgradeManager;


	[Header("Animation")]
	[SerializeField] private RectTransform rectTransform;
	[SerializeField] private float timeToMove;
	[SerializeField] private Vector2 inactivePosition;
	[SerializeField] private Vector2 activePosition;
	private bool isAnimating = false;
	private bool goingOut = false;

	private Vector2 lastPosition;
	private Vector2 target;
	private float elapsedTime;

	public override bool LoadUIManager()
	{
		// UI
		GameController.Instance.m_OnMarkTower.AddListener(SelectObject);
		GameController.Instance.m_OnTowerDataUpdate.AddListener(UpdateUI);

		// Animation
		lastPosition = inactivePosition;
		target = activePosition;
		StartCoroutine(ToggleAnimation());

		return true;
	}

	public override void Toggle(bool value)
	{
		isAnimating = true;
		SwapAnimationTarget();
	}

	private void SelectObject(GameController.TowerSelection towerSelection) {
		selectedObject = towerSelection.towerGameObject;
		selectedTowerSO = towerSelection.towerSO;

		titleText.text = selectedTowerSO.name;
		portraitImage.sprite = selectedTowerSO.sprite;
	}

	private void UpdateUI(GameController.TowerData towerData) {
		killsText.text = towerData.kills.ToString();
		sellPriceText.text = string.Format("{0}{1}", 
			GameController.Instance.currency, 
			towerData.sellPrice
		);

		//upgradeManager.Update(towerData);
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
