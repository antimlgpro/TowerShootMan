using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;

public class Placing : MonoBehaviour
{
	[System.Serializable]
	public class BuildOptions {
		[Range(0f, 360f)] public float maxAngle;
		[Range(0f, 100f)] public float noBuildRadius;

		public LayerMask groundMask;
		public LayerMask overlapMask;
	}

	private const int allocatedColliders = 45;
	private Collider[] colliders;

	[Header("General")]
	[SerializeField] private bool buildMode;
	[HideInInspector] public bool BuildMode => buildMode;
	private Vector3 HitPoint;
	private Vector3 HitNormal;

	private TowerSO TowerSO;

	// GHOST
	[Header("Ghost")]
	private GameObject ghostObject;
	private RadiusSphere radiusSphere;
	[SerializeField] private Material ghostMaterial;


	// FLAGS
	private bool f_IsTowerSelected {
		get {
			return TowerSO != null;
		}
	}

	[SerializeField] private bool f_MouseOnIsland;

	[SerializeField] private BuildOptions buildOptions;

    void Start()
    {
		colliders = new Collider[allocatedColliders];

		ghostObject = null;
		radiusSphere = null;

		GameController.Instance.m_OnSelectBuildable.AddListener(OnSelectBuildable);
    }

	void OnSelectBuildable(TowerSO towerSO) {
		print(towerSO);

		if (towerSO == null) SetBuildMode(false);

		SelectTower(towerSO);
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("StartBuild")) {
			ToggleBuildMode();
		}

		if (!EventSystem.current.IsPointerOverGameObject()) return;

		if (Input.GetButtonDown("Fire1")) {
			Place();
		}
    }

	void FixedUpdate() {
		Raycast();

		if (!f_MouseOnIsland) {
			HideGhost();
		} else {
			ShowGhost();
		}

		MoveGhost(HitPoint, HitNormal);
	}

	void Raycast() {
		if (buildMode == false) return;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast (ray, out RaycastHit hit, 500, buildOptions.groundMask)) {
			HitPoint = hit.point;
			HitNormal = hit.normal;

			f_MouseOnIsland = true;
		} else {
			f_MouseOnIsland = false;
		}
	}

	void SelectTower(TowerSO towerSO) {
		TowerSO = towerSO;

		SetBuildMode(true);

		Reset();
	}

	void Reset() {
		KillGhost();
		SpawnGhost();
	}

	void ToggleBuildMode() {
		SetBuildMode(!buildMode);
	}

	void SetBuildMode(bool value) {
		buildMode = value;

		GameController.Instance.m_ToggleBuildMode.Invoke(buildMode);

		if (buildMode == false) {
			KillGhost();
		} else {
			Reset();
		}
	}

	void SpawnGhost() {
		if (f_IsTowerSelected == false) return; // Only spawn ghost if gameobject is selected.
		if (ghostObject != null) return;
		if (radiusSphere != null) return;

		ghostObject = Instantiate(TowerSO.prefab, HitPoint, Quaternion.identity);
		ghostObject.layer = LayerMask.NameToLayer("Ghost");

		radiusSphere = ghostObject.GetComponentInChildren<RadiusSphere>();
		radiusSphere.SetActive(true);
		radiusSphere.SetRadius(TowerSO.range);

		ghostObject.GetComponent<SetMaterialInChildren>().SetMaterial(new Material(ghostMaterial));
		ghostObject.GetComponent<ToggleError>().Error = false;
	}

	void HideGhost() {
		if (ghostObject == null) return;
		if (radiusSphere == null) return;

		ghostObject.SetActive(false);
		radiusSphere.SetActive(false);
	}

	void ShowGhost() {
		if (ghostObject == null) return;
		if (radiusSphere == null) return;

		ghostObject.SetActive(true);
		radiusSphere.SetActive(true);
	}

	void KillGhost() {
		if (ghostObject == null) return;
		if (radiusSphere == null) return;

		Destroy(ghostObject);

		ghostObject = null;
		radiusSphere = null;
	}


	void MoveGhost(Vector3 point, Vector3 normal) {
		if (ghostObject == null) return;
		if (radiusSphere == null) return;
		if (buildMode == false) ghostObject.transform.position = Vector3.zero;

		bool canPlace = ValidPlacement(point, normal);

		ghostObject.GetComponent<ToggleError>().Error = !canPlace;
			
		ghostObject.transform.position = point;
	}

	bool ValidPlacement(Vector3 point, Vector3 normal) {
		bool validAngle = false;
		bool validDistance = false;

		// Checks if angle is less then max allowed
		float angle = Mathf.Acos(Vector3.Dot(Vector3.up, normal) / Vector3.up.magnitude * normal.magnitude) * Mathf.Rad2Deg;
		validAngle = angle <= buildOptions.maxAngle;

		// Checks if any objects are too close to ghost
		int targets = Physics.OverlapSphereNonAlloc(point, buildOptions.noBuildRadius, colliders, buildOptions.overlapMask);
		validDistance = targets == 1;

		return validAngle && validDistance;
	}

	bool CanPlace() {
		if (f_IsTowerSelected == false) return false;

		bool isValidPlacement = ValidPlacement(HitPoint, HitNormal);
		bool canAfford = GameController.Instance.CanAfford(TowerSO.cost);

		return isValidPlacement && canAfford && f_MouseOnIsland;
	}

	bool Purchase(TowerSO towerSO) {
		return GameController.Instance.PurchaseTower(towerSO);
	}

	void Place() {
		if (buildMode == false) return;

		bool canPlace = CanPlace();
		if (canPlace == false) return;
		if (Purchase(TowerSO) == false) return;

		GameObject spawnedTower = Instantiate(TowerSO.prefab, HitPoint, Quaternion.identity);
		spawnedTower.GetComponent<Tower>().ToggleTower(true);
		spawnedTower.name = string.Format("Spawned {0}", TowerSO.name);
		spawnedTower.layer = LayerMask.NameToLayer("Tower");

		spawnedTower.GetComponentInChildren<RadiusSphere>().SetRadius(TowerSO.range);

		if (GameController.Instance.Preferences.DisableBuildModeOnBuild) {
			SetBuildMode(false);
		}
	}
}
