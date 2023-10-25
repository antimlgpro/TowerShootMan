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

	[Header("References")]
	// UI stuff
	[SerializeField] private Image backgroundReference;
	[SerializeField] private TextMeshProUGUI costReference;
	

	[Header("Default")]
	// Default stuff
	[SerializeField] private Sprite defaultSprite;
	[SerializeField] private string currency;


	public void Initialize(TowerObject _towerObject) {
		towerObject = _towerObject;
		
		towerCost = _towerObject.cost;
		towerSprite = _towerObject.sprite;
		UpdateReferences();
	}

	void Start() {
		towerCost = 0;
		towerSprite = defaultSprite;
		UpdateReferences();
	}

	void UpdateReferences() {
		backgroundReference.sprite = towerSprite;
		costReference.text = currency + towerCost.ToString();
	}
}
