using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
	[SerializeField] private TowerObject towerObject;

	[SerializeField] private GameObject gunPivot;
	[SerializeField] private GameObject projectilePoint;
	[SerializeField] private GameObject projectileObject;

	[SerializeField] private EnemySpawner enemySpawner;

	private List<GameObject> enemies;


	private Vector3 towerResetPosition;
	private Vector3 gunResetPosition;

	void Start()
	{
		enemies = enemySpawner.enemies;

		towerResetPosition = transform.position;
		gunResetPosition = gunPivot.transform.position;
	}

	// true if target was successfully aquired. 
	// false if it failed.
	bool AquireTarget(GameObject enemy) {
		Vector3 enemyPos = enemy.transform.position;

		float distanceToEnemy = Vector3.Distance(transform.position, enemyPos);
		if (!(distanceToEnemy <= towerObject.range)) return false;

		Vector3 targetPostition = new( 
			enemyPos.x, 
        	transform.position.y, 
            enemyPos.z
		);
		transform.LookAt(targetPostition, Vector3.up);

		gunPivot.transform.LookAt(enemyPos, Vector3.up);


		if (Physics.Linecast(gunPivot.transform.position, enemyPos, out RaycastHit hit))
		{
			if (!hit.collider.gameObject.CompareTag("Enemy")) return false;
		}

		return true;
	}

	void ShootTarget(GameObject enemy) {
		GameObject projectile = Instantiate(projectileObject, projectilePoint.transform.position, Quaternion.identity);
		projectile.GetComponent<Rigidbody>().AddForce(gunPivot.transform.forward * 150, ForceMode.Impulse);
	}

	void ResetGun() {
		transform.position = towerResetPosition;
		gunPivot.transform.position = gunResetPosition;
	}


	// Aquire target should return a list of valid targets for this frame that can be sorted or selected based on criterias.
	void Update()
	{
		foreach (var enemy in enemies)
		{
			bool success = AquireTarget(enemy);
			if (!success) continue;

			if (success) ShootTarget(enemy);
			break;
		}
	}
}
