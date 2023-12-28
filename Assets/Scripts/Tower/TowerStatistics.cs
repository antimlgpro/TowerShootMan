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
		radiusSphere = GetComponentInChildren<RadiusSphere>();
		tower = GetComponent<Tower>();


		GameController.Instance.m_UpgradeOnSelect.AddListener(OnSelect);
		GameController.Instance.m_UpgradeOnDeselect.AddListener(OnDeselect);
    }

	public Guid GetGUID() {
		return GUID;
	}

	// If target equals this tower guid. we got selected
	void OnSelect(Guid targetGUID) {
		if (targetGUID.Equals(GUID)) {
			isSelected = true;
			radiusSphere.SetActive(true);
			Debug.LogFormat("Successfully selected {0}", GUID.ToString());

			GameController.Instance.m_OnMarkTower.Invoke(new GameController.TowerSelection(gameObject, tower.towerObject));
		} else {
			isSelected = false;
			radiusSphere.SetActive(false);
			Debug.LogFormat("Successfully deselected {0}", GUID.ToString());
		}
	}

	// if this is ran all towers are deselected.
	void OnDeselect() {
		isSelected = false;
		radiusSphere.SetActive(false);
		Debug.LogFormat("Successfully deselected {0}", GUID.ToString());
	}
}
