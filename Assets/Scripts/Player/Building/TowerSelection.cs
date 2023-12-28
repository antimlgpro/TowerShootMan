using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSelection : MonoBehaviour
{
	[SerializeField] private LayerMask selectionLayerMask;

	private Placing placingReference;

	private GameObject currentSelectedObject;
	private Guid currentSelectedGuid;
	private bool towerIsMarked;
	

    // Start is called before the first frame update
    void Start()
    {
        placingReference = GetComponent<Placing>();
    }

    // Update is called once per frame
    void Update()
	{
		// Only detect towers when not building.
		if (placingReference.BuildMode == false) FindTowerAtMouse();


		if (Input.GetButtonDown("Fire1")) {
			SelectTower();
		}
	}

	private void FindTowerAtMouse()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out RaycastHit hit, 500, selectionLayerMask))
		{
			// To prevente using GetComponent every frame mouse is hovering
			if (currentSelectedObject == hit.transform.gameObject) return;
			currentSelectedObject = hit.transform.gameObject;
			currentSelectedGuid = currentSelectedObject.GetComponent<TowerStatistics>().GetGUID();
			towerIsMarked = true;
	
		} else {
			towerIsMarked = false;
			currentSelectedObject = null;
		}
	}

	private void SelectTower() {
		if (towerIsMarked) {
			Debug.LogFormat("selecting tower {0}", currentSelectedGuid.ToString());
			GameController.Instance.m_UpgradeOnSelect.Invoke(currentSelectedGuid);
		} else {
			GameController.Instance.m_UpgradeOnDeselect.Invoke();
		}
	}
}
