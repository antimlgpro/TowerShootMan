using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerProjectile : Tower
{
	[SerializeField] private Animation placeAnimation;

	[SerializeField] private TowerSO towerObject;
	private bool towerEnabled = false;
	[SerializeField] private bool TowerEnabled => towerEnabled;

	[SerializeField] private GameObject gunPivot;
	[SerializeField] private GameObject projectilePoint;
	[SerializeField] private GameObject projectileObject;

	[SerializeField] private EnemySpawner enemySpawner;
	[SerializeField]
	private float noTargetCooldown = 1.5f;

	[SerializeField, Range(1, 40)]
	private int maxTargets;

	private Vector3 towerResetPosition;
	private Vector3 gunResetPosition;

	Collider[] colliders;
	Transform[] targets;


	void Start()
	{
		towerResetPosition = transform.position;
		gunResetPosition = gunPivot.transform.position;
		colliders = new Collider[maxTargets];
		targets = new Transform[maxTargets];
		StartCoroutine(TowerLoop());
	}

	public override void ToggleTower(bool value)
	{
		towerEnabled = value;
		placeAnimation.Play();
	}
	
	// Not filtered
	Transform[] GetTargetsInRange(Vector3 position, float range = 80f) {
		int targets = Physics.OverlapSphereNonAlloc(position, range, colliders);

		Transform[] targetArray = new Transform[targets];
		for (int i = 0; i < targets; i++)
		{
			targetArray[i] = colliders[i].transform;
		}

		return targetArray;
	}
	
	// Also ignores inactive targets, ie dead.
	Transform[] FilterTargetsByTag(Transform[] targets, string tag) {
		return targets.Where(x => {
			if (!x.CompareTag(tag)) return false;
			if (!x.gameObject.activeSelf) return false;

			return true;
		}).ToArray();
	}

	Transform[] ValidateTargets(Transform[] targets) {
		return targets.Where(x => {
			return !Physics.Linecast(projectilePoint.transform.position, x.position, 1 << 3);
		}).ToArray();
	}

	public Vector3 LeadTarget(Vector3 initialProjectilePosition, Vector3 targetPosition, Vector3 targetVelocity, float projectileSpeed) {
		float distance = Vector3.Distance(initialProjectilePosition, targetPosition);
		float travelTime = distance / projectileSpeed;

		return targetPosition + targetVelocity * travelTime;
	}

	public void AimAtTarget(Vector3 targetPosition) {
		Vector3 targetPostition = new( 
			targetPosition.x, 
        	transform.position.y, 
            targetPosition.z
		);
		transform.LookAt(targetPostition, Vector3.up);
		gunPivot.transform.LookAt(targetPosition, Vector3.up);
	}


	void ShootTarget(Transform target) {
		// Leading the target
		Vector3 targetPosition = target.transform.position;
		float velocity = 100f;
		targetPosition = LeadTarget(projectilePoint.transform.position, targetPosition, target.GetComponent<Velocity>().velocity, velocity);

		AimAtTarget(targetPosition);

		// Spawn projectile and push it towards target
		GameObject projectile = Instantiate(projectileObject, projectilePoint.transform.position, Quaternion.identity);
		projectile.GetComponent<Projectile>().Initialize(towerObject.damage);
		projectile.GetComponent<Rigidbody>().AddForce(gunPivot.transform.forward * velocity, ForceMode.Impulse);
	}

	protected override IEnumerator TowerLoop() {
		while (true) {
			if (towerEnabled) {
				targets = ValidateTargets(
					FilterTargetsByTag(
						GetTargetsInRange(transform.position, towerObject.range), 
						"Enemy")
					);

				if (targets.Length >= 1) {
					Debug.DrawLine(projectilePoint.transform.position, targets[0].position, Color.red, 1);
					ShootTarget(targets[0]);
				}
			}

			// Time in seconds between shots
			yield return new WaitForSeconds(1 / (towerObject.RPM / 60));
		}
	}
}
