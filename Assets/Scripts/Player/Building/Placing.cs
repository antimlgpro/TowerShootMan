using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placing : MonoBehaviour
{
	private TowerObject towerObject;

	private GameObject tower;
	private GameObject ghostObject;

	private const int layerMask = 1 << 3;
	private const int overlapLayerMask = ~(1 << 3 | 1 << 7); // All but layer 3 and 7
	private Collider[] colliders;

	// Degrees
	[SerializeField] private float maxAngle = 45f;
	[SerializeField] private float maxDistanceToAnotherTower = 30f;


	[Header("Building checks")]

	// True if currently building.
	[SerializeField] private bool validTowerSelection;
	[SerializeField] private bool buildMode;
	[SerializeField] private bool canPlace;
	[SerializeField] private bool ghostHidden;
	// True if mouse is on island. Used to prevent placing towers when clicking buttons in buildmode.
	[SerializeField] private bool mouseOnIsland;
	private bool ghostActive;

    // Start is called before the first frame update
    void Start()
    {
		// Magic number means max colliders checked. Should in theory never need more than this.
		colliders = new Collider[25];

		GameController.Instance.m_OnSelectBuildable.AddListener(OnSelectBuildable);
    }

	void Update() {
		if (Input.GetButtonDown("StartBuild")) {
			ToggleBuildMode();
		}

		if (Input.GetButtonDown("Fire1")) {
			Place();
		}
	}

	void OnSelectBuildable(TowerObject _towerObject) {
		// The tower was deselected or invalid
		if (_towerObject == null) {
			Debug.Log("Invalid tower selected. This might be bad.");

			// If the player selects invalid object or deselects a tower we 
			// turn off buildmode and propagate it to the gamecontroller.
			validTowerSelection = false;
			buildMode = false;
			HideGhost();
			GameController.Instance.m_ToggleBuildMode.Invoke(buildMode);
			return;
		}

		towerObject = _towerObject;
		tower = _towerObject.prefab;

		validTowerSelection = true;

		ToggleBuildMode(false);
		SpawnGhost(_towerObject.prefab);
	}

	// If toggle == false buildmode is set to true
	void ToggleBuildMode(bool toggle = true) {
		if (validTowerSelection == false) return;

		if (toggle) {
			buildMode = !buildMode;
		} else {
			buildMode = true;
		}

		GameController.Instance.m_ToggleBuildMode.Invoke(buildMode);

		SpawnGhost(tower);
		if (buildMode == false) {
			// Hide ghostobject when not building
			HideGhost();
		}
	}

    void FixedUpdate()
    {
        MoveGhost();
    }

	void SpawnGhost(GameObject prefab) {
		if (tower == null) return;
		if (validTowerSelection == false) return;
		if (ghostActive) return;

		ghostObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
		canPlace = false;

		ghostActive = true;
		ghostHidden = true;
	}

	void MoveGhost() {
		if (!buildMode) return;
		if (!ghostActive) return;
		if (validTowerSelection == false) return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray, out RaycastHit hit, 500, layerMask)) {
			canPlace = CanPlace(hit);
			mouseOnIsland = true;
			ghostObject.GetComponent<ToggleError>().Error = !canPlace;

			ghostObject.transform.position = hit.point;
			ghostHidden = false;
        } else {
			mouseOnIsland = false;
			HideGhost();
		}
	}

	void HideGhost() {
		ghostObject.transform.position = new Vector3(0, 0, 0);
		ghostHidden = true;
	}

	bool CanPlace(RaycastHit hit) {
		bool validPlacement;
		bool validAngle;
		bool validDistance;
		bool validMouse;

		// Checks if angle is less then max allowed
		float angle = Mathf.Acos(Vector3.Dot(Vector3.up, hit.normal) / Vector3.up.magnitude * hit.normal.magnitude) * Mathf.Rad2Deg;
		validAngle = angle <= maxAngle;

		// Checks if any objects are too close to ghost
		int targets = Physics.OverlapSphereNonAlloc(hit.point, maxDistanceToAnotherTower, colliders, overlapLayerMask);
		validDistance = targets == 1;

		// Checks if mouse is in a valid position
		validMouse = mouseOnIsland;
		
		// This will be "anded" with multiple placement checks in future.
		validPlacement = validAngle && validDistance && validMouse;
		return validPlacement;
	}

	// Places current object
	void Place() {
		if (tower == null) return;
		if (validTowerSelection == false) return;
		if (canPlace == false) return;
		if (buildMode == false) return;
		if (ghostActive == false) return;
		if (ghostHidden == true) return;

		// Could not purchase tower.
		if (GameController.Instance.PurchaseTower(towerObject) == false) {
			return;
		}

		// TODO: This could be a setting. IE only allow placing once per buildmode toggle.
		// This requires creating a new ghost object.
		//buildMode = false;
		ghostActive = false;

		ghostObject.GetComponent<Tower>().ToggleTower();
		ghostObject.name = "Placed Tower";

		ghostObject = null;

		// See todo above.
		SpawnGhost(tower);
	}	
}
