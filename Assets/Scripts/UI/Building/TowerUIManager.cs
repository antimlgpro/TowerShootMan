using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowerUIManager : UIManagerBase
{
	[SerializeField] private GameObject prefab;

	[SerializeField] private List<TowerObject> towerObjects;
	[SerializeField] private List<GameObject> towerElements;


	public UnityEvent<GameObject> m_OnSelectTower;

    public override bool LoadUIManager()
    {
		if (towerObjects.Count < 1) return false;

		if (!CreateElements()) return false;

		m_OnSelectTower ??= new();

		m_OnSelectTower.AddListener(OnSelectBuildable);

        return true;
    }

	void OnSelectBuildable(GameObject towerObject) {
		foreach (GameObject tower in towerElements) {
			if (tower == towerObject) {
				tower.GetComponent<TowerElement>().ToggleSelection();
			}
		}
	}

	bool CreateElements() {
		foreach (TowerObject towerObject in towerObjects) {
			GameObject towerElementObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
			towerElements.Add(towerElementObject);
			towerElementObject.transform.parent = transform;
			TowerElement element = towerElementObject.GetComponent<TowerElement>();
			element.Initialize(towerObject, this);
		}

		return true;
	}

}
