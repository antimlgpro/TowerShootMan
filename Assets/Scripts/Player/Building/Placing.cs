using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placing : MonoBehaviour
{
	public GameObject tower;
	private GameObject ghostObject;

	private const int layerMask = 1 << 3;

	// True if currently building.
	[SerializeField] private bool buildMode;
	[SerializeField] private bool canPlace;
	private bool ghostActive;

	
	// Degrees
	[SerializeField] private float maxAngle = 45f;

    // Start is called before the first frame update
    void Start()
    {
		SpawnGhost(tower);
    }

	void Update() {
		if (Input.GetButtonDown("StartBuild")) {
			buildMode = !buildMode;

			SpawnGhost(tower);
		}

		if (Input.GetButtonDown("Fire1")) {
			Place();
		}
	}

    void FixedUpdate()
    {
        MoveGhost();
    }

	void SpawnGhost(GameObject prefab) {
		if (ghostActive) return;

		ghostObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
		canPlace = false;

		ghostActive = true;
	}

	void MoveGhost() {
		if (!buildMode) return;
		if (!ghostActive) return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray, out RaycastHit hit, 500, layerMask)) {
			// if (!hit.transform.CompareTag("Island")) return;

			canPlace = CanPlace(hit);
			ghostObject.GetComponent<ToggleError>().Error = !canPlace;

			ghostObject.transform.position = hit.point;
        }
	}

	bool CanPlace(RaycastHit hit) {
		bool validPlacement;
		bool validAngle;

		// Checks if angle is less then max allowed
		float angle = Mathf.Acos(Vector3.Dot(Vector3.up, hit.normal) / Vector3.up.magnitude * hit.normal.magnitude) * Mathf.Rad2Deg;
		validAngle = angle <= maxAngle;

		
		// This will be "anded" with multiple placement checks in future.
		validPlacement = validAngle;
		return validPlacement;
	}

	// Places current object
	void Place() {
		if (canPlace == false) return;
		if (buildMode == false) return;
		if (ghostActive == false) return;

		buildMode = false;
		ghostActive = false;


		ghostObject.GetComponent<Tower>().ToggleTower();

		ghostObject = null;
	}	
}
