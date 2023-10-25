using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUIManager : UIManagerBase
{
	[SerializeField] private GameObject prefab;

	[SerializeField] private List<TowerObject> towerObjects;

    public override bool LoadUIManager()
    {
		if (towerObjects.Count < 1) return false;

		if (!CreateElements()) return false;

        return true;
    }

	bool CreateElements() {
		try {
			foreach (TowerObject towerObject in towerObjects) {
				GameObject towerElementObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
				towerElementObject.transform.parent = transform;
				TowerElement element = towerElementObject.GetComponent<TowerElement>();
				element.Initialize(towerObject);
			}
		} catch {
			return false;
		}

		return true;
	}

}
