using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TowerUIManager : UIManagerBase
{
	[SerializeField] private GameObject prefab;

	[SerializeField] private List<TowerSO> towerObjects;
	private List<GameObject> towerElements;
	private string selectedTowerName;


	[SerializeField] private TextMeshProUGUI selectedTowerText;

	public UnityEvent<GameObject> m_OnSelectTower;

    public override bool LoadUIManager()
    {
		if (towerObjects.Count < 1) return false;

		towerElements = new List<GameObject>();

		if (!CreateElements()) return false;

		m_OnSelectTower ??= new();
		m_OnSelectTower.AddListener(OnSelectBuildable);

		GameController.Instance.m_ToggleBuildMode.AddListener(OnToggleBuildMode);

        return true;
    }

	void OnSelectBuildable(GameObject towerObject) {
		if (towerObject != null) {
			selectedTowerName = towerObject.GetComponent<TowerElement>().towerObject.name;
			selectedTowerText.text = selectedTowerName;
		} else {
			selectedTowerText.text = "";
		}

		foreach (GameObject tower in towerElements) {
			if (tower == towerObject) {
				tower.GetComponent<TowerElement>().ToggleSelection();
				continue;
			}

			tower.GetComponent<TowerElement>().ToggleSelection(false);
		}
	}

	void OnToggleBuildMode(bool value) {
		// If buildmode is disabled we deselect all tower elements.
		if (value == false) {
			OnSelectBuildable(null);
		}
	}

	bool CreateElements() {
		foreach (TowerSO towerObject in towerObjects) {
			GameObject towerElementObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
			towerElements.Add(towerElementObject);
			towerElementObject.transform.parent = transform;

			TowerElement element = towerElementObject.GetComponent<TowerElement>();
			element.Initialize(towerObject, this);
		}

		return true;
	}

}
