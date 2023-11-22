using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerElement : MonoBehaviour
{
	[SerializeField] private TowerObject towerObject;
	private int towerCost;
	private Sprite towerSprite;

	private TowerUIManager towerUIManager;
	private bool selected;

	[Header("References")]
	// UI stuff
	[SerializeField] private Image backgroundReference;
	[SerializeField] private Image selectBorderReference;
	[SerializeField] private TextMeshProUGUI costReference;
	[SerializeField] private Button buttonReference;
	

	[Header("Default")]
	// Default stuff
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private string currency;


	public void Initialize(TowerObject _towerObject, TowerUIManager _towerUIManager) {
		towerObject = _towerObject;
		towerUIManager = _towerUIManager;
		
		towerCost = _towerObject.cost;
		towerSprite = _towerObject.sprite;
		UpdateReferences();

		selected = false;

		buttonReference.onClick.AddListener(Select);
	}

	void Select() {
		towerUIManager.m_OnSelectTower.Invoke(gameObject);

		if (selected) {
			// Select this tower
			GameController.Instance.m_OnSelectBuildable.Invoke(towerObject);
		} else {
			// Deselect
			GameController.Instance.m_OnSelectBuildable.Invoke(null);
		}
	}

	public void ToggleSelection() {
		selected = !selected;
		selectBorderReference.enabled = selected;
	}

	void UpdateReferences() {
		backgroundReference.sprite = towerSprite;
		costReference.text = currency + towerCost.ToString();
	}
}
