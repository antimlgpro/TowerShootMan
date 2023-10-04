using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tower : MonoBehaviour
{
	[SerializeField] private TowerObject towerObject;

	[SerializeField] private GameObject gunPivot;
	[SerializeField] private GameObject projectilePoint;
	[SerializeField] private GameObject projectileObject;

	[SerializeField] private EnemySpawner enemySpawner;

	[SerializeField, Range(1, 40)]
	private int maxTargets;

	private Vector3 towerResetPosition;
	private Vector3 gunResetPosition;

	void Start()
	{
		towerResetPosition = transform.position;
		gunResetPosition = gunPivot.transform.position;

		StartCoroutine(ShootLoop());
	}

	Transform[] AquireTargets() {
		Collider[] colliders = new Collider[maxTargets];
		int targets = Physics.OverlapSphereNonAlloc(transform.position, towerObject.range, colliders);

		Transform[] targetArray = new Transform[targets];
		for (int i = 0; i < targets; i++)
		{
			targetArray[i] = colliders[i].transform;
		}

		return targetArray.Where(x => {
			if (!x.CompareTag("Enemy")) return false;
			if (!x.gameObject.activeSelf) return false;

			return true;
		}).ToArray();
	}

	void ShootTarget(Transform target) {
		Vector3 targetPos = target.transform.position;

		Vector3 targetPostition = new( 
			targetPos.x, 
        	transform.position.y, 
            targetPos.z
		);
		transform.LookAt(targetPostition, Vector3.up);
		gunPivot.transform.LookAt(targetPos, Vector3.up);

		GameObject projectile = Instantiate(projectileObject, projectilePoint.transform.position, Quaternion.identity);
		projectile.GetComponent<Projectile>().Initialize(towerObject.damage);
		projectile.GetComponent<Rigidbody>().AddForce(gunPivot.transform.forward * 150, ForceMode.Impulse);
	}

	void ResetGun() {
		transform.position = towerResetPosition;
		gunPivot.transform.position = gunResetPosition;
	}

	
	[SerializeField]
	private float noTargetCooldown = 1.5f;


	IEnumerator ShootLoop() {
		while (true) {
			Transform[] targets = AquireTargets();

			if (targets.Length >= 1) {
				ShootTarget(targets[0]);
			}

			// Time in seconds between shots
			yield return new WaitForSeconds(1 / (towerObject.RPM / 60));
		}
	}
}
