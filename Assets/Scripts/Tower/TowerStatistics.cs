using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TowerStatistics : MonoBehaviour
{
	public Guid GUID;

	private bool isSelected = false;
	private RadiusSphere radiusSphere;
	private Tower tower;

    // Start is called before the first frame update
    void Start()
    {
		GUID = Guid.NewGuid();
        isSelected = false;
    }

	void OnSelect(Guid targetGUID) {
		if (targetGUID.Equals(GUID)) {
			isSelected = true;
			radiusSphere.SetActive(true);
			Debug.LogFormat("Successfully selected {0}", GUID.ToString());

			GameController.Instance.m_OnMarkTower.Invoke(new GameController.TowerSelection(gameObject, tower.towerObject));
		}
	}

	void OnDeselect(Guid targetGUID) {
		if (targetGUID.Equals(GUID)) {
			isSelected = false;
			radiusSphere.SetActive(false);
			Debug.LogFormat("Successfully deselected {0}", GUID.ToString());
		}
	}
}
